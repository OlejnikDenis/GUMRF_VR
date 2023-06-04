using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    ///<summary>
    /// This class represents the editor window for spawning prefabs.
    ///</summary>
    public class PrefabSpawnerEditor : EditorWindow
    {
        ///<summary>
        /// Data structure for storing information about the prefab to spawn and its spawning parameters.
        ///</summary>
        private class PrefabSpawnData
        {
            public GameObject Prefab;                // The prefab object to be spawned.
            public int Rows;                         // Number of rows for spawning.
            public int Columns;                      // Number of columns for spawning.
            public float RowSpacing;                 // Spacing between rows.
            public float ColSpacing;                 // Spacing between columns.
            public Transform ParentTransform;        // The parent transform under which the spawned prefabs will be placed.
            public List<GameObject> SpawnedPrefabs;  // List of the spawned prefabs.
        }

        private GameObject _prefab;
        private int _rows = 5;
        private int _columns = 5;
        private float _rowSpacing = 2f;
        private float _colSpacing = 2f;
        private Transform _parentTransform;
        private readonly List<PrefabSpawnData> _spawnedPrefabDataList = new List<PrefabSpawnData>();

        #region [Unity] System method calls
        ///<summary>
        /// Shows the Prefab Spawner window in the Unity Editor.
        ///</summary>
        [MenuItem("Custom/Prefab Spawner")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<PrefabSpawnerEditor>("Prefab Spawner");
        }

        private void OnGUI()
        {
            #region [Spawn] Fields generator
            _prefab = EditorGUILayout.ObjectField("Prefab", _prefab, typeof(GameObject), false) as GameObject;
            EditorGUILayout.Space(5);

            GUILayout.Label("Sizes", EditorStyles.boldLabel);
            _rows = EditorGUILayout.IntSlider("Rows", _rows, 1, 100);
            _columns = EditorGUILayout.IntSlider("Columns", _columns, 1, 100);
            EditorGUILayout.Space(5);

            GUILayout.Label("Spacing", EditorStyles.boldLabel);
            _rowSpacing = EditorGUILayout.Slider("Row Spacing", _rowSpacing, 0.001f, 10f);
            _colSpacing = EditorGUILayout.Slider("Column Spacing", _colSpacing, 0.001f, 10f);
            EditorGUILayout.Space(5);

            GUILayout.Label("Parent", EditorStyles.boldLabel);
            _parentTransform = EditorGUILayout.ObjectField("Parent Transform", _parentTransform, typeof(Transform), true) as Transform;
            #endregion
            
            if (GUILayout.Button("Spawn Prefabs"))
            {
                SpawnPrefabs();
            }

            foreach (var prefabData in _spawnedPrefabDataList)
            {
                #region [Update] Fields generator
                EditorGUILayout.Space(10);

                GUILayout.Label($"Prefab Set {prefabData.ParentTransform.name}", EditorStyles.boldLabel);
                prefabData.Prefab = EditorGUILayout.ObjectField("Prefab", prefabData.Prefab, typeof(GameObject), false) as GameObject;
                EditorGUILayout.Space(5);

                GUILayout.Label("Sizes", EditorStyles.boldLabel);
                prefabData.Rows = EditorGUILayout.IntSlider("Rows", prefabData.Rows, 1, 100);
                prefabData.Columns = EditorGUILayout.IntSlider("Columns", prefabData.Columns, 1, 100);
                EditorGUILayout.Space(5);

                GUILayout.Label("Spacing", EditorStyles.boldLabel);
                prefabData.RowSpacing = EditorGUILayout.Slider("Row Spacing", prefabData.RowSpacing, 0.001f, 10f);
                prefabData.ColSpacing = EditorGUILayout.Slider("Column Spacing", prefabData.ColSpacing, 0.001f, 10f);
                EditorGUILayout.Space(5);

                GUILayout.Label("Parent", EditorStyles.boldLabel);
                prefabData.ParentTransform = EditorGUILayout.ObjectField("Parent Transform", prefabData.ParentTransform, typeof(Transform), true) as Transform;
                #endregion
                
                if (GUILayout.Button("Update"))
                {
                    UpdatePrefabData(prefabData);
                }
            }
        }

        ///<summary>
        /// Handles the hierarchy change event by removing null or destroyed spawned prefabs from the list.
        ///</summary>
        private void OnHierarchyChange()
        {
            for (int i = _spawnedPrefabDataList.Count - 1; i >= 0; i--)
            {
                var prefabData = _spawnedPrefabDataList[i];
                
                // Iterate over the spawned prefabs in reverse order to safely remove null or destroyed prefabs.
                if (prefabData.SpawnedPrefabs != null)
                {
                    for (int j = prefabData.SpawnedPrefabs.Count - 1; j >= 0; j--)
                    {
                        var spawnedPrefab = prefabData.SpawnedPrefabs[j];
                        
                        // Check if the spawned prefab is null or has been destroyed.
                        if (spawnedPrefab == null)
                        {
                            // Remove the null or destroyed prefab from the spawned prefabs list.
                            prefabData.SpawnedPrefabs.RemoveAt(j);
                        }
                    }
                }
                
                // If there are no more spawned prefabs in the list, remove the prefab data from the main list.
                if (prefabData.SpawnedPrefabs == null || prefabData.SpawnedPrefabs.Count == 0)
                {
                    _spawnedPrefabDataList.RemoveAt(i);
                }
            }
        }
        #endregion
        
        
        ///<summary>
        /// Spawns the prefabs based on the specified parameters.
        ///</summary>
        private void SpawnPrefabs()
        {
            if (_prefab == null)
            {
                Debug.LogError("Prefab is not assigned!");
                return;
            }

            var prefabData = new PrefabSpawnData
            {
                Prefab = _prefab,
                Rows = _rows,
                Columns = _columns,
                ColSpacing = _colSpacing,
                RowSpacing = _rowSpacing,
                ParentTransform = _parentTransform
            };

            SpawnOrUpdatePrefabs(prefabData);
        }

        ///<summary>
        /// Spawns or updates prefabs based on the provided prefab data.
        ///</summary>
        private void SpawnOrUpdatePrefabs(PrefabSpawnData prefabData)
        {
            // Check if the prefab is not assigned.
            if (prefabData.Prefab == null)
            {
                Debug.LogError("Prefab is not assigned!");
                return;
            }

            // Clear the previously spawned prefabs associated with the prefab data.
            ClearSpawnedPrefabs(prefabData);

            // Create a new list to store the spawned prefabs.
            prefabData.SpawnedPrefabs = new List<GameObject>(prefabData.Rows * prefabData.Columns);

            // If the parent transform is not assigned, create a new GameObject as the parent transform.
            if (prefabData.ParentTransform == null)
            {
                int spawnedIndex = _spawnedPrefabDataList.Count + 1;
                prefabData.ParentTransform = new GameObject($"[{spawnedIndex}] Spawned {prefabData.Prefab.name}").transform;
            }
            
            // Instantiate the prefabs based on the prefab data.
            InstantiatePrefabWithData(prefabData);

            // Add the prefab data to the list of spawned prefab data if it doesn't already exist.
            if (!_spawnedPrefabDataList.Contains(prefabData))
            {
                _spawnedPrefabDataList.Add(prefabData);
            }

            Debug.Log("Prefabs spawned/updated in scene.");
        }

        ///<summary>
        /// Updates the prefabs based on the provided prefab data.
        ///</summary>
        private void UpdatePrefabData(PrefabSpawnData prefabData)
        {
            // Check if the prefab is not assigned.
            if (prefabData.Prefab == null)
            {
                Debug.LogError("Prefab is not assigned!");
                return;
            }

            // Check if there are no spawned prefabs to update.
            if (prefabData.SpawnedPrefabs == null || prefabData.SpawnedPrefabs.Count == 0)
            {
                Debug.LogWarning("No spawned prefabs to update!");
                return;
            }

            // Clear the previously spawned prefabs.
            ClearSpawnedPrefabs(prefabData);

            // Create a new list to hold the updated spawned prefabs.
            prefabData.SpawnedPrefabs = new List<GameObject>(prefabData.Rows * prefabData.Columns);

            // Spawn or update the prefabs based on the updated prefab data.
            SpawnOrUpdatePrefabs(prefabData);

            Debug.Log("Prefabs updated in scene.");
        }

        ///<summary>
        /// Instantiates prefabs based on the provided prefab data.
        ///</summary>
        private void InstantiatePrefabWithData(PrefabSpawnData prefabData)
        {
            for (var col = 0; col < prefabData.Columns; col++)
            {
                for (var row = 0; row < prefabData.Rows; row++)
                {
                    // Calculate the spawn position based on the column, row, spacing, and parent transform position.
                    var spawnPosition = new Vector3(col * prefabData.ColSpacing, 0f, row * prefabData.RowSpacing) +
                                        prefabData.ParentTransform.position;
                    // Instantiate the prefab using PrefabUtility.InstantiatePrefab and cast it to a GameObject.
                    var spawnedPrefab =
                        PrefabUtility.InstantiatePrefab(prefabData.Prefab, prefabData.ParentTransform) as GameObject;
                    
                    // Check if the prefab instantiation failed.
                    if (spawnedPrefab == null)
                    {
                        Debug.LogError("Failed to spawn a prefab.");
                        return;
                    }

                    // Set the position of the spawned prefab.
                    spawnedPrefab.transform.position = spawnPosition;
                    
                    // Generate a unique name for the spawned prefab based on its index, row, and column.
                    var spawnIndex = _spawnedPrefabDataList.Count + 1;
                    spawnedPrefab.name = $"{prefabData.Prefab.name} [{spawnIndex}] ({row}, {col})";
                    
                    // Add the spawned prefab to the list of spawned prefabs in the prefab data.
                    prefabData.SpawnedPrefabs.Add(spawnedPrefab);
                }
            }
        }

        ///<summary>
        /// Clears the spawned prefabs from the scene based on the provided prefab data.
        ///</summary>
        private void ClearSpawnedPrefabs(PrefabSpawnData prefabData)
        {
            // Check if there are spawned prefabs to clear.
            if (prefabData.SpawnedPrefabs is not {Count: > 0}) return;
            
            // Iterate over the spawned prefabs and destroy them using DestroyImmediate.
            foreach (var spawnedPrefab in prefabData.SpawnedPrefabs.Where(spawnedPrefab => spawnedPrefab != null))
            {
                DestroyImmediate(spawnedPrefab);
            }

            // Clear the list of spawned prefabs in the prefab data.
            prefabData.SpawnedPrefabs.Clear();
        }
    }
}
