using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LineProcessor : Editor
{
    static List<Vector3> vertices;
    static List<int> lineSegments;
    static string inputFilePath;
    static string inputFilename;

    [MenuItem( "Assets/Process Line File" )]
    static void ProcessFile( ) {

        Object[] textAssets = Selection.GetFiltered( typeof( TextAsset ), SelectionMode.Unfiltered );

        foreach ( Object textAsset in textAssets ) {
            TextAsset objFile = (TextAsset)textAsset;
            inputFilePath = AssetDatabase.GetAssetPath(objFile);
            inputFilename = objFile.name;

            vertices = new List<Vector3>();
            lineSegments = new List<int>();

            string text = objFile.text;
            string[] allLines = text.Split( '\n' );
            for (int i = 0; i < allLines.Length; i++)
            {
                var line = allLines[i];

                //Clean double spaces that some programs export for some reason
                line = line.Replace("  ", " ");

                //Parse verts
                if (line.StartsWith("v"))
                {
                    string[] vertexLineSplit = line.Split(' ');
                    Vector3 v = new Vector3(float.Parse(vertexLineSplit[1]), float.Parse(vertexLineSplit[2]), float.Parse(vertexLineSplit[3]));
                    vertices.Add(v);
                }

                //Parse lines
                if (line.StartsWith("l"))
                {
                    List<int> lineSegment = new List<int>();

                    string[] lineSplit = line.Split(' ');
                    foreach (string l in lineSplit)
                    {
                        int index;
                        if (int.TryParse(l, out index))
                        {
                            lineSegment.Add(index);
                        }
                    }
                    lineSegment.Add(int.MinValue);

                    if (lineSegment.Count > 0)
                    {
                        lineSegments.AddRange(lineSegment);
                    }
                }
            }

            SaveLineFile();
        }
    }

    static void SaveLineFile() 
    {
        LineModel lm = CreateInstance<LineModel>();
        string modelFolderPath = Path.GetDirectoryName(inputFilePath) + "/LineModels";
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(modelFolderPath + "/" + inputFilename + ".asset");

        if (!Directory.Exists(modelFolderPath))
            Directory.CreateDirectory(modelFolderPath);

        AssetDatabase.CreateAsset( lm, assetPathAndName );

        AssetDatabase.StartAssetEditing();
        lm.vertices = vertices.ToArray();
        lm.lineSegments = lineSegments.ToArray();
        AssetDatabase.StopAssetEditing();

        EditorUtility.SetDirty( lm );
        AssetDatabase.SaveAssets();

        SaveMesh( lm );
    }

    static void SaveMesh( LineModel lineModel ) {
        Mesh mesh = new Mesh();

        if ( lineModel != null ) {
            List<int> indexList = new List<int>();
            for ( int i = 0; i < lineModel.lineSegments.Length - 1; ++i ) {
                int index = lineModel.lineSegments[i];
                int nextIndex = lineModel.lineSegments[i + 1];

                if ( index != int.MinValue && nextIndex != int.MinValue ) {
                    indexList.Add( index - 1 );
                    indexList.Add( nextIndex - 1 );
                }
            }
            mesh.vertices = lineModel.vertices;
            mesh.SetIndices( indexList.ToArray(), MeshTopology.Lines, 0 );
            Color[] colors = new Color[mesh.vertexCount];
            for ( int i = 0; i < colors.Length; ++i ) {
                colors[i] = new Color( 1f, 1f, 1f, 1f );
            }
            mesh.colors = colors;

            string meshFolderPath = Path.GetDirectoryName(inputFilePath) + "/LineMeshes";
            string meshPathAndName = AssetDatabase.GenerateUniqueAssetPath(meshFolderPath + "/" + inputFilename + ".asset");

            if (!Directory.Exists(meshFolderPath))
                Directory.CreateDirectory(meshFolderPath);

            AssetDatabase.CreateAsset(mesh, meshPathAndName);
            AssetDatabase.SaveAssets();
        }
    }
}
