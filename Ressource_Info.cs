///<summary> Ressource Info script counts ressources in containers and represents them on a Wide LCD Panel
///<author> Cranc - Debauchery Engineer Tea Party

//default container name
string container_name = "Container Ingots";
string lcd_name = "Ressource Display";

//defines how many lines have space on one panel
const int panel_lines = 15;

//makros
const string cargo_name = "--cargo:";
const string cargo_bool = "--c-exact:";
const string display_name = "--display:";
const string display_bool = "--d-exact:";
const string delimiter = ",";

void Main(String argument)
{
    // init stuff + default container
    bool exact = false;
    bool exact_LCD = false;

    var container_list = new List<IMyTerminalBlock>();
    var lcd_panel_list = new List<IMyTerminalBlock>();

    var container_inventory_list = new List<IMyInventory>();

    var item_list = new List<IMyInventoryItem>();

    var dict = new Dictionary<string, VRage.MyFixedPoint>();

    // get name and exact value for container and display
    if(argument != null){
        var pos = argument.IndexOf(cargo_name);
        if(pos != -1){
            var epos = argument.IndexOf(delimiter);
            if(epos != -1){
                container_name = argument.Substring(pos + cargo_name.Length, epos - (pos + cargo_name.Length));
            }else{
                container_name = argument.Substring(pos + cargo_name.Length);
            }
            argument = argument.Substring(epos + 1);
        }
    }

    if(argument != null){
        var pos = argument.IndexOf(cargo_bool);
        if(pos != -1){
            var epos = argument.IndexOf(delimiter);
            var temp = "nope";
            if(epos != -1){
                temp = argument.Substring(pos + cargo_bool.Length, epos - (pos + cargo_bool.Length));
            }else{
                temp = argument.Substring(pos + cargo_bool.Length);
            }
            if(temp.Contains("true") || temp.Contains("1") || temp.Contains("True"))
                exact = true;
            argument = argument.Substring(epos + 1);
        }
    }

    if(argument != null){
        var pos = argument.IndexOf(display_name);
        if(pos != -1){
            var epos = argument.IndexOf(delimiter);
            if(epos != -1){
                lcd_name = argument.Substring(pos + display_name.Length, epos - (pos + display_name.Length));
            }else{
                lcd_name = argument.Substring(pos + display_name.Length);
            }
            argument = argument.Substring(epos + 1);
        }
    }

    if(argument != null){
        var pos = argument.IndexOf(display_bool);
        if(pos != -1){
            var epos = argument.IndexOf(delimiter);
            var temp = "";
            if(epos != -1){
                temp = argument.Substring(pos + display_bool.Length, epos - (pos + display_bool.Length));
            }else{
                temp = argument.Substring(pos + display_bool.Length);
            }
            if(temp.Contains("true") || temp.Contains("1") || temp.Contains("True"))
                exact_LCD = true;
            argument = argument.Substring(epos + 1);
        }
    }

    /// container list
    if(!exact){
        // get container contain at least the string
        GridTerminalSystem.SearchBlocksOfName(container_name, container_list, FilterMethod);
    }else{
        // get container contain exact string
        GridTerminalSystem.SearchBlocksOfName(container_name, container_list, ExactFilterMethod);
    }

    if(container_list.Count == 0)
        throw new Exception("no container found (" + container_name + ") ... Someone fucked uuuup");

    /// LCD Panels
    if(!exact_LCD){
        //get LCD contain att least the string
        GridTerminalSystem.SearchBlocksOfName(lcd_name, lcd_panel_list, FilterMethodLCD);
    }else{
        GridTerminalSystem.SearchBlocksOfName(lcd_name, lcd_panel_list, ExactFilterMethodLCD);
    }

    if(lcd_panel_list.Count == 0)
        throw new Exception("no LCD-Panels (" + lcd_name + ") found ... Someone fucked uuuup");

    /// get all container inventorys
    for(int i = 0; i < container_list.Count; i++){
        container_inventory_list.Add(container_list[i].GetInventory(0));
    }

    if(container_inventory_list.Count == 0)
        throw new Exception("container inventory list empty ... huh???");

    ///set up dictionary
    dict = new Dictionary<string, VRage.MyFixedPoint>();

    dict.Add("IronMyObjectBuilder_Ingot",(VRage.MyFixedPoint)0);
    dict.Add("GoldMyObjectBuilder_Ingot",(VRage.MyFixedPoint)0);
    dict.Add("CobaltMyObjectBuilder_Ingot",(VRage.MyFixedPoint)0);
    dict.Add("NickelMyObjectBuilder_Ingot",(VRage.MyFixedPoint)0);
    dict.Add("PlatinumMyObjectBuilder_Ingot",(VRage.MyFixedPoint)0);
    dict.Add("SilverMyObjectBuilder_Ingot",(VRage.MyFixedPoint)0);
    dict.Add("MagnesiumMyObjectBuilder_Ingot",(VRage.MyFixedPoint)0);
    dict.Add("SiliconMyObjectBuilder_Ingot",(VRage.MyFixedPoint)0);
    dict.Add("UraniumMyObjectBuilder_Ingot",(VRage.MyFixedPoint)0);
    dict.Add("StoneMyObjectBuilder_Ingot",(VRage.MyFixedPoint)0);

    //Main stuff

    for(int i = 0; i < lcd_panel_list.Count; i++){
        var panel = lcd_panel_list[0] as IMyTextPanel;
        panel.ShowPublicTextOnScreen();

        if(i == 0)
            panel.WritePublicText("Ressources in Container \n");
    }

    for(int j = 0; j < container_inventory_list.Count; j++){
        item_list = container_inventory_list[j].GetItems();

        for(int i = 0; i < item_list.Count; i++){
            var content = item_list[i].Content;
            string itemname = content.SubtypeName.ToString() + content.TypeId.ToString();
            if(dict.ContainsKey(itemname)){
                dict[itemname] += item_list[i].Amount;
            }
        }
    }

    int k = 0;
    foreach(string key in dict.Keys){
        int panel_nr = 0;

        if(k > panel_lines)
            panel_nr++;

        if(panel_nr >= lcd_panel_list.Count)
            throw new Exception("you ran out of space O.O build more panels or change const ... Someone fucked uuuup");

        StringBuilder name = new StringBuilder(key.Replace("MyObjectBuilder_"," "));
        name.Append("'s: ");
        name.Append(dict[key].ToString());
        name.Append(" kg");

        var panel = lcd_panel_list[panel_nr] as IMyTextPanel;
        panel.WritePublicText(name + "\n", append: true);

        k++;
    }
}

///<param name="block"> block to be checked
bool FilterMethod(IMyTerminalBlock block)
{
    IMyCargoContainer cargo = block as IMyCargoContainer;
    return cargo != null;
}

///<param name="block"> block to be checked
bool ExactFilterMethod(IMyTerminalBlock block)
{
    IMyCargoContainer cargo = block as IMyCargoContainer;
    if(cargo != null){
        if(cargo.CustomName.Equals(container_name))
            return true;
    }
    return false;
}

///<param name="block"> block to be checked
bool FilterMethodLCD(IMyTerminalBlock block)
{
    IMyTextPanel panel = block as IMyTextPanel;
    return panel != null;
}

///<param name="block"> block to be checked
bool ExactFilterMethodLCD(IMyTerminalBlock block)
{
    IMyTextPanel panel = block as IMyTextPanel;
    if(panel != null){
        if(panel.CustomName.Equals(lcd_name))
            return true;
    }
    return false;
}