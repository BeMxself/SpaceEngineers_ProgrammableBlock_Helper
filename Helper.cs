IMyTerminalBlock GetBlock(string name){    
    IMyTerminalBlock block = GridTerminalSystem.GetBlockWithName(name); 
    if (block==null) 
        throw new Exception(String.Format("\"{0}\" Not Found", name)); 
    return block; 
}    
T GetBlock<T>(string name){
    IMyTerminalBlock block = GetBlock(name);  
    return (T)block;
}
void RunAction(IMyTerminalBlock block, string actionName){    
    block.GetActionWithName(actionName).Apply(block);
}    
    
void RunAction(string blockName, string actionName){    
    RunAction(GetBlock(blockName), actionName);    
}    
    
void BatchRunAction(IList<string> blockNames, string actionName){    
    for(int i = 0; i < blockNames.Count; ++i)    
        RunAction(blockNames[i], actionName);    
}    
    
void BatchRunAction(IList<IMyTerminalBlock> blocks, string actionName){    
    for(int i = 0; i < blocks.Count; ++i)    
        RunAction(blocks[i], actionName);    
} 

class Var{
    private IMyGridTerminalSystem gts;
    private IMyTerminalBlock varBlock;
    private string[] varArray;
    private string separator;
    public Var(IMyGridTerminalSystem gts){
        this.gts = gts;
        InitVar("$var", "|");
    }
    public Var(IMyGridTerminalSystem gts,string prefix){ 
        this.gts = gts;
        InitVar(prefix, "|");
    }
    public Var(IMyGridTerminalSystem gts,string prefix, string separator){ 
        this.gts = gts;
        InitVar(prefix, separator);
    }

    private void InitVar(string prefix, string separator){ 
        this.separator = separator;
        varBlock = FirstBlockWithPrefix(prefix); 
        varArray = varBlock.CustomName.Split(new string[]{separator}, StringSplitOptions.None);
    }   
    private IMyTerminalBlock FirstBlockWithPrefix(string prefix){    
        List<IMyTerminalBlock> allBlocks = gts.Blocks;   
        for (int i = 0; i < allBlocks.Count; ++i){   
            if (allBlocks[i].CustomName.StartsWith(prefix))   
                return allBlocks[i];   
        }   
        throw new Exception(String.Format("FirstBlockWithPrefix: prefix \"{0}\" not found", prefix));   
    }
    
    public T GetVar<T>(){
        return GetVar<T>(0);
    }
    public T GetVar<T>(uint index){
        return (T)(Object)varArray[index+1];
    }
    public T GetVarWithDefault<T>(object defaultValue){
        return GetVarWithDefault<T>(0, defaultValue);
    }
    public T GetVarWithDefault<T>(uint index, object defaultValue){
        try{
            return GetVar<T>(index);
        }
        catch(Exception e){
            return (T)defaultValue;
        }
    }

    public void SetVar(object value){
        SetVar(0, value);
    }
    public void SetVar(uint index, object value){
        if (index+2 > varArray.Length)
            Array.Resize(ref varArray, (int)index+2);
        varArray[index+1] = value == null ? "" : value.ToString();
        varBlock.SetCustomName(String.Join(separator, varArray));
    }
}

