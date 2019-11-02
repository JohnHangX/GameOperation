using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.UI;
using System.IO;
using System.Runtime.InteropServices;
using System;

public class GamesManager : MonoBehaviour {

    string fileDir;
    public Text showText;
    public delegate void ListenerHandler();
    public event ListenerHandler Listener;
    private Process curGame;
    private EventHandler exitGameHandler;
    DirectoryInfo[] gamesDics;
    List<string> gamesList = new List<string>();
    int curGameIndex = 0;
    void Start () {
        Application.runInBackground = true;
        //exitGameHandler = new EventHandler(NextProgress);
        string tempPath = @"E:\release\GamesOperateDemo_Data";
        //DirectoryInfo di = new DirectoryInfo(Application.dataPath);
        DirectoryInfo di = new DirectoryInfo(tempPath);
        fileDir = di.Parent.FullName + "/Games/";
        DirectoryInfo diParent = new DirectoryInfo(fileDir);
        gamesDics = diParent.GetDirectories();
        foreach (DirectoryInfo gameDir in gamesDics)
        {
            gamesList.Add(gameDir.Name);
            print(gameDir.Name);
        }
        //showText.text = fileDir;
        curGameIndex = 0;
        SelectGame();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    //operType参数如下：
    //0: 隐藏, 并且任务栏也没有最小化图标  
    //1: 用最近的大小和位置显示, 激活  
    //2: 最小化, 激活  
    //3: 最大化, 激活  
    //4: 用最近的大小和位置显示, 不激活  
    //5: 同 1  
    //6: 最小化, 不激活  
    //7: 同 3  
    //8: 同 3  
    //9: 同 1  
    //10: 同 1 
    [DllImport("kernel32.dll")]
    public static extern int WinExec(string exeName, int operType);

    [DllImport("user32.dll")]
    public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

    void SelectGame()
    {
        string gameName = gamesList[curGameIndex];
        string exeName = fileDir + gameName + "/" + gameName + ".exe";
        StartProgress(exeName, gameName);
    }

    void StartProgress(string path, string gameName)
    {
        //ProcessStartInfo info = new ProcessStartInfo();
        //info.FileName = path;
        //info.Arguments = "";
        //info.WindowStyle = ProcessWindowStyle.Minimized;
        //print(path);
        //Process pro = Process.Start(info);
        //SwitchToThisWindow(pro.MainWindowHandle, true);
        //pro.WaitForExit();

        WinExec(path, 4);
        AddExitProcessFunc(gameName);


    }

    void AddExitProcessFunc(string gameName)
    {
        Process[] temp = Process.GetProcessesByName(gameName);
        if (temp.Length > 0)//如果查找到
        {
            //IntPtr handle = temp[0].MainWindowHandle;
            //SwitchToThisWindow(handle, true); // 激活，显示在最前
            curGame = temp[0];
            //curGame.WaitForExit();
            
            curGame.Exited += exitGameHandler;
            if (curGame.HasExited == true) NextProgress();
            curGame.EnableRaisingEvents = true;
            print(curGame.ProcessName);
            //NextProgress();
        }
        else
        {
            Process.Start(gameName + ".exe");//否则启动进程
            AddExitProcessFunc(gameName);
        }
    }

    void NextProgress(object obj, EventArgs e)
    {
        print("next progress");
        curGame.Exited -= exitGameHandler;
        int gamesLen = gamesList.Count;
        curGameIndex += 1;
        if (curGameIndex >= gamesLen)
        {
            curGameIndex = 0;
        }
        SelectGame();
    }
    void NextProgress()
    {
        print("next progress");
        curGame.Exited -= exitGameHandler;
        int gamesLen = gamesList.Count;
        curGameIndex += 1;
        if (curGameIndex >= gamesLen)
        {
            curGameIndex = 0;
        }
        SelectGame();
    }

    void OnApplicationQuit()
    {
        print("OnApplicationQuit");
        if (curGame != null)
            curGame.Exited -= exitGameHandler;
    }
}
