using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Terminal : MonoBehaviour
{
    public TextMeshProUGUI history;
    public TMP_InputField currentCommand;
    public GameObject eventSystem;

    public Directory currentDirectory;
    public Directory tempDirectory;
    public List<Directory> directories = new List<Directory>();    
    public List<string> historyCommands = new List<string>();
    public List<string> packages = new List<string>();
    public int index;

    public GameObject finishPanel, possibleAddresses, usage;

    public bool nmap = false, netTools = false, password = false, sshUpdate = false, pwdbreaker = false;

    public bool startMenu = false;

    public bool poweredOn;

    public int missed = 0;

    // Todo add directorys so the user can read directories

    public void Start()
    {
        // Set to Root Directory
        currentDirectory = directories[0];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {            
            ReadData();
            historyCommands.Add(currentCommand.text);
            currentCommand.text = "";
            index = historyCommands.Count;
            currentCommand.ActivateInputField(); //Re-focus on the input field
            currentCommand.Select();//Re-focus on the input field
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (historyCommands.Count > 0)
            {
                if (index > 0)
                {
                    index--;
                }
                else
                    index = historyCommands.Count - 1;

                currentCommand.text = historyCommands[index];
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (historyCommands.Count > 0)
            {
                if (index < historyCommands.Count - 1)
                {
                    index++;
                }
                else
                    index = 0;

                currentCommand.text = historyCommands[index];
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.C))
        {
            password = false;
            tempDirectory = null;
            history.text += "\n";
            currentDirectory = directories.Find(e => e.directoryName == "192.168.0.2");
        }
    }

    // Add in a hint command :)

    // Read command and print out da
    public void ReadData()
    {
        string[] command = currentCommand.text.Trim().ToLower().Split(' ');

        bool found = true;

        if (password)
        {
            if (string.Join(" ", command) == tempDirectory.password)
            {
                ChangeDirectory(tempDirectory.directoryName);
                history.text += "\nConnection Successful";
                password = false;
                tempDirectory = null;
            }
            else
            {
                history.text += "\n<color=red>Error, Password incorrect.</color> \nRoot@" + tempDirectory.directoryName + " Enter Password:";
                missed++;

                if (missed % 2 == 0)
                {
                    tempDirectory = null;
                    password = false;
                    history.text += "\nConnection Failed Unauthorized Acess\nRoot@" + currentDirectory.directoryName;
                }
            }
            return;
        }

        if (startMenu)
        {
            if (string.Join(" ", command) == "run gitgud.exe")
            {
                SceneManager.LoadScene("GameScene");
                return;
            }
            else if (string.Join(" ", command) == "exit")
                Application.Quit();
        }

        switch (command[0])
        {
            case ("help"):
                history.text = "\nAvaliable Commands:\n apt-list,               apt-install," +
                                                    "\n apt-update,        break," +
                                                    "\n exit,                    cd," +
                                                    "\n clear,                  color," +
                                                    "\n ifconfig,              hello," +
                                                    "\n help,                   ls," +
                                                    "\n mkdir,                 nmap," +
                                                    "\n rm,                     ssh," +
                                                    "whoami";
                break;
            case ("apt-install"):
                Install(command);
                break;
            case ("apt-update"):
                AptUpdate(command);
                break;
            case ("apt-list"):
                history.text += "\nInstalled Packages:"; foreach (string x in packages) { history.text += "\n " + x; }
                break;
            case ("clear"):
                history.text = "";
                if (historyCommands.Count > 0)
                    historyCommands.Clear();
                break;
            case ("color"):
                SwapColor(command);
                break;
            case ("nmap"):
                if (nmap) Nmap(command); else history.text += "\n<color=red>Error, nmap not installed try apt-install nmap.</color>";
                break;
            case ("net-tools"):
                if (netTools) history.text += "\nNet-Tools is up to date."; else found = false;
                break;
            case ("ifconfig"):
                if (netTools) IFconig(command); else history.text += "\n<color=red>Error, net-tools not installed try apt-install net-tools.</color>";
                break;
            case ("ssh"):
                if (sshUpdate)
                {
                    SSH(command);
                    usage.SetActive(true);
                }
                else
                    history.text += "\n<color=red>Error: packages out of date, update your packages to continue</color>";
                break;
            case ("ls"):
                PrintDirectory();
                break;
            case ("cd"):
                ChangeDirectory(command);
                break;
            case ("mkdir"):
                MakeDirectory(command);
                break;
            case ("whoami"):
                history.text += "\nCurrent User: " + currentDirectory.currentUser;
                break;
            // Wrong Commands
            case ("ipconig"):
            case ("dir"):
            case ("cls"):
                history.text += "\n<color=orange>I AM NOT WINDOWS BASED CHIEF.</color>";
                break;
            // Jokes
            case ("sudo"):
                history.text += "\nshhhhh, pretend that doesn't exist I didn't want to code that...";
                break;
            case ("hello"):
                history.text += "\nworld.";
                break;
            case ("rm"):
                Remove(command);
                break;
            case ("git"):
                history.text += "\nShhhhhh I haven't been invented yet silly.";
                break;
            case ("break"):
            case ("exit"):
                if (currentDirectory.directoryName == "192.168.0.2")
                {
                    history.text += "\nBruh. you're already in your home directory";
                }
                else
                {
                    history.text += "\nRoot@192.168.0.2/";
                    currentDirectory = directories.Find(e => e.directoryName == "192.168.0.2");
                }
                break;
            case ("pwdbreaker"):
                if (pwdbreaker) PWDBreaker(command); else history.text += "\nError, pwdbreaker not installed. try apt-install pwdbreaker";
                break;
            default:
                found = false;
                break;
        }    

        if (!found)
        {
            missed += 1;
            history.text += "\n<color=red>Error: '" + currentCommand.text + "', not found  </color>";
        }
        else
            missed = 0;

        if (!found && missed % 3 == 0)
            history.text += "\nLooks like you're struggling here chief, try help....";
    }
    // apt-install
    public void Install(string[] package)
    {
        if (package.Length > 3)
        {
            history.text += "\n<color=red>Error: " + string.Join(" ", package) + ", not found  </color>";
            return;
        }
        else if (package.Length == 1)
        {
            history.text += "\n<color=red>Error: 'expected paramater', try apt-install list  </color>";
            return;
        }

        // Downloads
        switch (package[1])
        {
            case ("list"):
                history.text += "\nAvalible Packages:\n  nmap, net-tools, pwdbreaker";
                break;
            case ("nmap"):                
                StartCoroutine(downloadHandeler(1, "nmap"));
                if (usage != null)
                    usage.SetActive(true);
                nmap = true;
                packages.Add("nmap");
                break;
            case ("net-tools"):
                StartCoroutine(downloadHandeler(1, "net-tools"));
                netTools = true;
                packages.Add("net-tools");
                break;
            case ("pwdbreaker"):
                StartCoroutine(downloadHandeler(1, "pwdbreaker"));
                if (usage != null)
                    usage.SetActive(true);
                pwdbreaker = true;
                packages.Add("pwdbreaker");
                break;
            default:
                history.text += "\n<color=red>Error: " + package[1] + " not found, try apt-install list for avalible downloads</color>";
                break;
        }
    }
    // Update
    public void AptUpdate(string[] cmd)
    {
        if (cmd.Length == 1)
        { 
            StartCoroutine(downloadHandeler(1, "updated packages"));
            sshUpdate = true;
        }
        else
            history.text += "\n<color=red>Error: too many arguments " + string.Join(" ", cmd) + "</color>";
    }
    // Color
    public void SwapColor(string[] cmd)
    {
        if (cmd.Length == 1)
        {
            history.text += "\n<color=red>Error: 'expected paramater', try color list  </color>";
            return;
        }

        bool found = true;

        // Downloads
        switch (cmd[1])
        {
            case ("white"):
                history.color = Color.white;
                currentCommand.textComponent.color = Color.white;
                break;
            case ("black"):
                history.color = Color.black;
                currentCommand.textComponent.color = Color.black;
                break;
            case ("red"):
                history.color = Color.red;
                currentCommand.textComponent.color = Color.white;
                break;
            case ("blue"):
                history.color = Color.blue;
                currentCommand.textComponent.color = Color.blue;
                break;
            case ("cyan"):
                history.color = Color.cyan;
                currentCommand.textComponent.color = Color.cyan;
                break;
            case ("grey"):
                history.color = Color.grey;
                currentCommand.textComponent.color = Color.grey;
                break;
            case ("gray"):
                history.color = Color.gray;
                currentCommand.textComponent.color = Color.gray;
                break;
            case ("magenta"):
                history.color = Color.magenta;
                currentCommand.textComponent.color = Color.magenta;
                break;
            case ("yellow"):
                history.color = Color.yellow;
                currentCommand.textComponent.color = Color.yellow;
                break;
            case ("green"):
                history.color = Color.green;
                currentCommand.textComponent.color = Color.green;
                break;
            case ("list"):
                history.text += "\nAvaliable Colors: white, black, red, blue, cyan, grey, gray, magenta, yellow, green";
                return;
            default:
                found = false;
                break;
        }

        if (found)
            history.text += "\n" + string.Join(" ", cmd);
        else
            history.text += "\n<color=red>Error: " + string.Join(" " , cmd) + ", not found try color list to see avaliable colors </color>";
    }
    // Nmap
    public void Nmap(string[] map)
    {
        if (map.Length == 1)
        {
            history.text += "\n<color=red>Error: try using nmap list</color>";
            return;
        }
        // Downloads
        switch (map[1])
        {
            case ("list"):
                history.text += "\nList of Avalible networks: 192.168.1.1, https://mit.edu";
                break;
            case ("localhost"):
                history.text += "\nYou're just a nerd arn't you";
                break;
            case ("192.168.1.1"):
                StartCoroutine(downloadHandeler(2, "nmap", '.', done =>
                {
                    history.text += "\nAvalible Connections: \n192.168.0.2, 192.168.0.8, 192.168.1.6, 192.168.1.9, 192.168.2.3";
                }));
                if (possibleAddresses != null)
                    possibleAddresses.SetActive(true);
                break;
            case ("https://mit.edu"):
                StartCoroutine(downloadHandeler(2, "nmap", '.', done =>
                {
                    history.text += "\n<html>\n  <body>\n    Welcome class of '89\n  </body>\n</html>\nAvalible Connections: \n192.168.0.2, 192.168.0.8, 192.168.1.6, 192.168.1.9, 192.168.2.3";
                }));
                if (possibleAddresses != null)
                    possibleAddresses.SetActive(true);
                break;
            default:
                history.text += "\n<color=red>Error: " + string.Join(" ", map) + ", is not a command, try nmap list instead</color>";
                break;
        }
    }
    public void PWDBreaker(string[] cmd) {
        if (cmd.Length == 2)
        {
            Directory temp = directories.Find(e => e.directoryName == cmd[1]);
            if (temp != null)
            {
                StartCoroutine(PasswordBreaker(temp.directoryName, temp.password, done =>
                {
                    history.text += "\n\nPassword for user @ " + temp.directoryName + " - " + temp.password; 
                }));
            }
            else
                history.text += "\n<color=red>throw new 'Null ref exception', reason: no address with that ip was found</color>";
        }
        else
            history.text += "\n<color=red>Error, try using pwdbreaker <ip_address></color>";
    }
    public void IFconig(string[] cmd)
    {
        if (cmd.Length == 1)
        {
            history.text += "\nifconfig\n" +
               "\neth0 Link encap:Ethernet  HWaddr 00:0F:20:CF:8B:42" +
               "\ninet addr: <color=yellow>192.168.0.2(Your PC)</color> Mask: 255.255.255.192" +
               "\nient public IP: <color=yellow>192.168.1.1(public ip)</color>" +
               "\nUP BROADCAST RUNNING MULTICAST  MTU: 1500  Metric: 1" +
               "\nRX packets:2472694671 errors: 1 dropped: 0 overruns: 0 frame: 0" +
               "\nTX packets:44641779 errors: 0 dropped: 0 overruns: 0 carrier: 0" +
               "\ncollisions: 0 txqueuelen: 1000" +
               "\nRX bytes:1761467179(1679.7 Mb)  TX bytes:2870928587(2737.9 Mb)" +
               "\nInterrupt: 28";
        }
        else
            history.text += "\n<color=red>Error: " + string.Join(" " , cmd) + ", not found</color>";
    }
    //SSH 
    public void SSH(string[] cmd)
    {
        if (cmd.Length == 1)
        {
            history.text += "\nusage: ssh address port";
            return;
        }
        switch (cmd[1])
        {
            case ("192.168.0.2"):
                if (cmd.Length == 2)
                {
                    history.text += string.Join(" ", cmd) + "\n<color=red>Error, no port specified. \ntry again with the following - ssh address portnumber</color>";
                    return;
                }
                else if (cmd[2] == "22")
                {
                    Password(cmd[1]);
                }
                break;
            case ("192.168.0.8"):
                if (cmd.Length == 2)
                {
                    history.text += string.Join(" ", cmd) + "\n<color=red>Error, no port specified. \ntry again with the following - ssh address portnumber</color>";
                    return;
                }
                else if (cmd[2] == "22")
                {
                    Password(cmd[1]);
                }
                break;
            case ("192.168.1.6"):
                if (cmd.Length == 2)
                {
                    history.text += string.Join(" ", cmd) + "\n<color=red>Error, no port specified. \ntry again with the following - ssh address portnumber</color>";
                    return;
                }
                else if (cmd[2] == "22")
                {
                    Password(cmd[1]);
                }
                break;
            case ("192.168.1.9"):
                if (cmd.Length == 2)
                {
                    history.text += string.Join(" ", cmd) + "\n<color=red>Error, no port specified. \ntry again with the following - ssh address portnumber</color>";
                    return;
                }
                else if (cmd[2] == "22")
                {
                    Password(cmd[1]);
                }
                break;
            case ("192.168.2.3"):
                if (cmd.Length == 2)
                {
                    history.text += string.Join(" ", cmd) + "\n<color=red>Error, no port specified. \ntry again with the following - ssh address portnumber</color>";
                    return;
                }
                else if (cmd[2] == "22")
                {
                    Password(cmd[1]);
                }
                break;
            default:
                history.text += string.Join(" ", cmd) + "\n<color=red>Error, no port specified. \ntry again with the following - ssh address portnumber</color>";
                break;
        }
    }
    // Simulate changing directory
    public void ChangeDirectory(string[] cmd)
    {
        if (cmd.Length == 1)
        {
            history.text += "\n" + string.Join(" ", cmd) + "\n<color=red>Error, expected command with cd such as .. or <Directory Name>.</color>";
            return;
        }
        else if (cmd[1] == "..")
        {
            if (directories.Find(e => e.directoryName == currentDirectory.parentDirectory) != null)
            {
                currentDirectory = directories.Find(e => e.directoryName == currentDirectory.parentDirectory);
                history.text += "\n\n" + currentDirectory.directoryName + "/";
            }
            else
                history.text += "\n<color=red>Error, at root directory</color>";
            return;
        }
        int index = 0;
        foreach (Directory childDir in currentDirectory.childDirectory)
        {
            if (childDir.directoryName == cmd[1])
            {
                currentDirectory = currentDirectory.childDirectory[index];
                history.text += "\n\n" + currentDirectory.directoryName + "/";
                return;
            }
            index++;
        }

        history.text += "\n<color=red>Error, No directory with the name " + string.Join(" ", cmd) + " could be found.</color>";
    }
    // Simulate changing directory
    public void ChangeDirectory(string directory)
    {
        currentDirectory = directories.Find(e => e.directoryName == directory);
        history.text += "\nRoot@" + directory + ":";
    }
    // Make a directory
    public void MakeDirectory(string[] cmd)
    {
        if (cmd.Length == 2)
        {
            Directory temp = new Directory(cmd[1]);
            temp.parentDirectory = currentDirectory.directoryName;
            currentDirectory.childDirectory.Add(temp);
            history.text += "\nDirectory: " + cmd[1] + "has been added";
        }
        else
            history.text += "\n<color=red>Error, no such command exists " + string.Join(" ", cmd) + ". try using mkdir directoryName.</color>";
    }
    // Password Protected
    public void Password(string connectionAddress)
    {
        var dir = directories.Find(e => (e.directoryName == connectionAddress));
        if (dir != null)
        {
            if (string.IsNullOrEmpty(dir.password))
            {
                currentDirectory = dir;
            }
            else
            {
                history.text += "\nRoot@" + connectionAddress + " Enter Password:";
                tempDirectory = dir;
                password = true;
            }
        }
    }
    // Print Directory
    public void PrintDirectory()
    {
        List<string> alphabeticalOrder = new List<string>();
        if (currentDirectory.files.Count > 0)
        {
            foreach (string file in currentDirectory.files)
            {
                alphabeticalOrder.Add(file);
            }
        }
        if (currentDirectory.childDirectory.Count > 0)
        {
            foreach (Directory childDir in currentDirectory.childDirectory)
            {
                alphabeticalOrder.Add(childDir.directoryName);
            }
        }

        history.text += "\nls\n";

        if (alphabeticalOrder.Count > 0)
        {
            alphabeticalOrder.Sort();

            foreach (string dir in alphabeticalOrder)
            {
                history.text += "\n  " + dir;
            }
        }
    }
    // Remove command
    public void Remove(string[] cmd)
    {
        if (cmd.Length == 2)
        {
            if (cmd[1] == "-rf")
            {
                StartCoroutine(deleteHandeler(1.2f, done =>
                {
                    if (currentDirectory.currentUser == "Dr Coleman")
                    {
                        // Play ending here
                        finishPanel.SetActive(true);
                        GameObject.FindObjectOfType<ScreenFade>().FinishScene();                        
                        return;
                    }
                    else if (currentDirectory.directoryName == "192.168.0.2")
                    {
                        gameObject.SetActive(false);
                        poweredOn = false;
                    }
                    else
                    {
                        history.text += "\nDisconnected from client, fatal error occured\nWell that wasn't Dr Colemans, hopefully they didn't need anything off of their PC ¯\\_(ツ)_/¯";
                        directories.Remove(currentDirectory);

                        currentDirectory = directories.Find(e => e.directoryName == "192.168.0.2");
                    }
                }));
            }
            else
                history.text += "\n<color=red>Error, no such command exists " + string.Join(" ", cmd) + ". try using rm -rf {!!!This is permanent and cannot be undone!!!}.</color>";
        }
        else if (cmd.Length == 1)
            history.text += "\n<color=red>Error, must use rm with force, try 'rm -rf' {!!!This is permanent and cannot be undone!!!}.</color>";
        else
            history.text += "\n<color=red>Error, no such command exists " + string.Join(" ", cmd) + ". try using rm -rf {!!!This is permanent and cannot be undone!!!}.</color>";
    }
    public void PowerPC()
    {
        if (!poweredOn)
        {
            poweredOn = true;
            gameObject.SetActive(true);
            history.text = "";
            if (historyCommands.Count > 0)
                historyCommands.Clear();
            currentCommand.text = "";
            StartCoroutine(startupHandeler(.5f));
            currentCommand.ActivateInputField(); //Re-focus on the input field
            currentCommand.Select();//Re-focus on the input field
        }
        else
        {
            poweredOn = false;
            gameObject.SetActive(false);
        }
        
    }
    // Download Handeler
    IEnumerator downloadHandeler(float amt)
    {
        float install = 0;

        history.text += "\nDownloading: \n";

        while (install < amt)
        {
            install += Time.deltaTime;
            history.text += "=";
            yield return new WaitForEndOfFrame();
        }
    }
    // Download Handeler
    IEnumerator startupHandeler(float amt)
    {
        float install = 0;

        history.text += "\nBooting: \n";

        while (install < amt)
        {
            install += Time.deltaTime;
            history.text += "=";
            yield return new WaitForSecondsRealtime(.1f);
        }

        history.text = "" +
"\nWW      WW         tt                         333333 " +
"\nWW      WW   aa aa tt    sss   oooo  nn nnn     3333 " +
"\nWW W    WW aa  aaa tttt  s    oo  oo nnn  nn   3333 " + 
"\n WW WWW WW aa  aaa tt    sss  oo  oo nn   nn     333 " +
"\n  WW   WW   aaa aa  tttt    s  oooo  nn   nn  333333 " +
 "\n                         sss " +
        "\n\n\n-Developed by Ran Crump";

        for (int i = 0; i < 4; i++)
        {
            history.text += "\n";
            yield return new WaitForSecondsRealtime(.5f);
        }
    }
    IEnumerator deleteHandeler(float amt, System.Action<bool> done)
    {
        float install = 0;

        history.text += "\nRecursively Deleting Files: \n";

        while (install < amt)
        {
            install += Time.deltaTime;
            history.text += "=";
            yield return new WaitForEndOfFrame();
        }

        history.text += "\nFinished...";

        done(true);
    }
    IEnumerator downloadHandeler(float amt, string message)
    {
        float install = 0;

        history.text += "\nDownloading: " + message + "\n";

        while (install < amt)
        {
            install += Time.deltaTime * 1.5f;
            history.text += "=";
            yield return new WaitForEndOfFrame();
        }

        history.text += "\n<color=yellow>" + message + " downloaded.</color>";
    }
    IEnumerator PasswordBreaker(string directory, string password, System.Action<bool> done)
    {
        float breaker = 0;
        int index = 0;

        yield return new WaitForSeconds(.25f);

        history.text += "\nPassword_Breaker: &currUser /*A-Z0-9*/ \n strcpy(buffer, argv[1]);";

        yield return new WaitForSeconds(.5f);

        history.text += "\n<color=red>Segmentation fault</color>";

        yield return new WaitForSeconds(.5f);

        history.text += "\nFor(;;) \nSelect * from curr_Users where name = " + directory + "\n";

        yield return new WaitForSeconds(.5f);

        float amt = 0;
        while (amt < 1)
        {
            amt += Time.deltaTime * 1.5f;
            history.text += "=";
            yield return new WaitForEndOfFrame();
        }

        history.text += "\nPass: ";

        while (breaker < password.Length)
        {            
            history.text += password.Substring(index, 1);
            breaker++;
            index++;
            yield return new WaitForSecondsRealtime(.5f);
        }

        done(true);
    }
    IEnumerator downloadHandeler(float amt, string message, char symbol, System.Action<bool> done)
    {
        float install = 0;

        history.text += "\nDownloading: " + message + "\n";

        while (install < amt)
        {
            install += Time.deltaTime * 1.5f;
            history.text += symbol;
            yield return new WaitForEndOfFrame();
        }

        history.text += "\n<color=yellow>" + message + " downloaded.</color>";

        done(true);
    }
}
[System.Serializable]
public class Directory
{
    public string directoryName = "";
    public string currentUser = "";
    public List<string> files = new List<string>();
    public string parentDirectory = null;
    public List<Directory> childDirectory = new List<Directory>();
    public string password = "";

    public Directory()
    {
        this.directoryName = "";
        files = new List<string> { };
        parentDirectory = null;
        childDirectory = new List<Directory>();
    }
    public Directory(string directoryName)
    {
        this.directoryName = directoryName;
        files = new List<string>();
        parentDirectory = null;
        childDirectory = new List<Directory>();
    }
}