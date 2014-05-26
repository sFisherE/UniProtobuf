using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.IO;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class CompilerMenu : Editor
{
    const string DllName = "ProtoGenData.dll";
    [MenuItem("Game Tools/Protobuf/Gen cs")]
    static void GenCS()
    {
        string path = Path.Combine(projectPath, "protobuf/protogen.bat");
        Process pro = new Process();
        FileInfo file = new FileInfo(path);
        pro.StartInfo.WorkingDirectory = file.Directory.FullName;
        pro.StartInfo.FileName = path;
        pro.StartInfo.CreateNoWindow = false;
        pro.Start();
        pro.WaitForExit();
    }
    [MenuItem("Game Tools/Protobuf/Create ProtoGen DLL")]
	static void createDLL()
	{
        Debug.Log("start compile");
		var compileParams = new CompilerParameters();

        var buildTargetFilename = DllName;

        compileParams.OutputAssembly = Path.Combine(Application.dataPath, "Plugins/"+buildTargetFilename);
		// for all available compiler options: http://msdn.microsoft.com/en-us/library/6ds95cz0(v=vs.80).aspx
		compileParams.CompilerOptions = "/optimize";
		compileParams.ReferencedAssemblies.Add( Path.Combine( EditorApplication.applicationContentsPath, "Managed/UnityEngine.dll" ) );
        compileParams.ReferencedAssemblies.Add(Path.Combine(Application.dataPath, "Plugins/protobuf-net.dll"));
		
		var source = getSourceForSelectedScripts();

		var codeProvider = new CSharpCodeProvider( new Dictionary<string, string> { { "CompilerVersion", "v3.0" } } );
    	var compilerResults = codeProvider.CompileAssemblyFromSource( compileParams, source );
		
    	if( compilerResults.Errors.Count > 0 )
    	{
    		foreach( var error in compilerResults.Errors )
    			Debug.LogError( error.ToString() );
		}
		else
		{
            //Debug.Log(buildTargetFilename + " should now in Plugins Folder.");
            AssetDatabase.Refresh();
            string path2 = Path.Combine(projectPath, "protobuf/precompile.bat");
            System.Diagnostics.Process.Start(path2);
            //System.Diagnostics.Process.WaitForExit();
            AssetDatabase.Refresh();
        }
	}

    static string projectPath
    {
        get
        {
            string path = Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length);
            return path;
        }
    }
	static string[] getSourceForSelectedScripts()
	{
        var source = new List<string>();
        string path = Path.Combine(projectPath, "protobuf/cs");
        DirectoryInfo di = new DirectoryInfo(path);
        FileInfo[] files = di.GetFiles("*.cs");
        foreach (var item in files)
        {
            //Debug.Log(item.ToString());
            StreamReader sr = item.OpenText();
            source.Add(sr.ReadToEnd());
        }
        return source.ToArray();
	}

}
