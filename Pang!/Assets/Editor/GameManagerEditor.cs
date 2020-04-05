using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GameManager gm = (GameManager)target;
        
        if(GUILayout.Button("Add Big Balloon"))
        {
            Instantiate(gm.BalloonsPrefabs[0], new Vector3(0, 4, 0), gm.BalloonsPrefabs[0].transform.rotation);
        }

        if (GUILayout.Button("Add Medium Balloon"))
        {
            Instantiate(gm.BalloonsPrefabs[1], new Vector3(0, 4, 0), gm.BalloonsPrefabs[0].transform.rotation);
        }

        if (GUILayout.Button("Add Small Balloon"))
        {
            Instantiate(gm.BalloonsPrefabs[2], new Vector3(0, 4, 0), gm.BalloonsPrefabs[0].transform.rotation);
        }

        if (GUILayout.Button("Add Tiny Balloon"))
        {
            Instantiate(gm.BalloonsPrefabs[3], new Vector3(0, 4, 0), gm.BalloonsPrefabs[0].transform.rotation);
        }
        
        if(GUILayout.Button("Remove ALL Balloons from Scene."))
        {
            GameObject[] allBalloons = GameObject.FindGameObjectsWithTag("Balloon");

            foreach (var balloon in allBalloons)
            {
                DestroyImmediate(balloon);
            }
        }
    }

}
