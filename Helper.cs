    void Main() {  
        InitBlockHelper(); 
        //codes below InitBlockHelper() is for test, you should remove this;
        ExNamedMem exmem = new ExNamedMem("$mem");
        exmem.SetVar("test_var","testpass"); 
        exmem.SetString("test_str", "testpass"); 
        exmem.SetInt("test_int", 123); 
        exmem.SetFloat("test_float", 3.14F); 
        string str = exmem.GetString("test_str"); 
        int aint = exmem.GetInt("test_int"); 
        float afloat= exmem.GetFloat("test_float");
    }  
         
    void InitBlockHelper(){ 
        BlockHelper.Init(GridTerminalSystem); 
    } 
    static class BlockHelper{ 
        static private IMyGridTerminalSystem GridTerminalSystem; 
        static public void Init(IMyGridTerminalSystem gts){ 
            GridTerminalSystem = gts; 
        } 
        static public IMyTerminalBlock GetBlock(string name) {   
            IMyTerminalBlock block = GridTerminalSystem.GetBlockWithName(name);   
            if (block == null)   
                throw new Exception(String.Format("\"{0}\" Not Found"));   
            return block;   
        }   
   
        static public void RunAction(IMyTerminalBlock block, string actionName) {   
            var action = block.GetActionWithName(actionName); 
            if (action == null) 
                throw new Exception(String.Format("Action \"{0}\" of \"{1}\" NOT FOUND!", actionName,  
                    block.CustomName)); 
            action.Apply(block);   
        }   
   
        static public void RunAction(string blockName, string actionName) {   
            RunAction(GetBlock(blockName), actionName);   
        }   
   
        static public void RunAction(IList<string> blockNames, string actionName) {   
            for (int i = 0; i < blockNames.Count; ++i)   
                RunAction(blockNames[i], actionName);   
        }   
   
        static public void RunAction(IList<IMyTerminalBlock> blocks, string actionName) {   
            for (int i = 0; i < blocks.Count; ++i)   
                RunAction(blocks[i], actionName);   
        }   
        static public IMyTerminalBlock FirstBlockWithPrefix(string prefix) {    
            List<IMyTerminalBlock> allBlocks = GridTerminalSystem.Blocks;    
            for (int i = 0; i < allBlocks.Count; ++i) {    
                if (allBlocks[i].CustomName.StartsWith(prefix))    
                    return allBlocks[i];    
            }    
            throw new Exception(String.Format("FirstBlockWithPrefix: prefix \"{0}\" not found", prefix));    
        }  
    }  
    class ExMem {  
        private IMyTerminalBlock varBlock;  
        private string[] varArray;  
        private string separator;  
        public ExMem() {  
            InitVar("$var", "|");  
        }  
        public ExMem(string prefix) {  
            InitVar(prefix, "|");  
        }  
        public ExMem(string prefix, string separator) {  
            InitVar(prefix, separator);  
        }  
        private void InitVar(string prefix, string separator) {  
            this.separator = separator;  
            varBlock = BlockHelper.FirstBlockWithPrefix(prefix);  
            varArray = varBlock.CustomName.Split(new string[] { separator }, StringSplitOptions.None);  
        }  
        public string GetString(){ 
            return GetString(0); 
        } 
        public string GetString(uint index){ 
            if (index + 2 > varArray.Length)   
                Array.Resize(ref varArray, (int)index + 2);  
            return varArray[index + 1]; 
        } 
        public int GetInt(){ 
            return GetInt(0); 
        } 
        public int GetInt(uint index){ 
            int value = 0;  
            int.TryParse(GetString(index), out value);  
            return value; 
        } 
        public float GetFloat(){ 
            return GetFloat(0); 
        } 
        public float GetFloat(uint index){  
            float value = 0;   
            float.TryParse(GetString(index), out value);   
            return value; 
        } 
        public void SetString(string value){ 
            SetString(0, value); 
        } 
        public void SetString(uint index, string value){ 
            if (index + 2 > varArray.Length)  
                Array.Resize(ref varArray, (int)index + 2);  
            varArray[index + 1] = value;  
            varBlock.SetCustomName(String.Join(separator, varArray));  
        } 
        public void SetVar(object value) {  
            SetVar(0, value);  
        }  
        public void SetVar(uint index, object value) {  
            SetString(index, Convert.ToString(value)); 
        }  
        public void SetInt(int value){ 
            SetInt(0,value); 
        } 
        public void SetInt(uint index, int value){ 
            SetString(index, Convert.ToString(value)); 
        } 
        public void SetFloat(float value){ 
            SetFloat(0,value); 
        } 
        public void SetFloat(uint index, float value){ 
            SetString(index, Convert.ToString(value)); 
        } 
    }  
    class ExNamedMem {    
        private string itemSep = "|";   
        private string nameSep = ":";  
        private string arraySep = ",";  
        private string memStr;  
        private string prefixStr = "$var"; 
        private Dictionary<string, string> memDic;  
 
        private IMyTerminalBlock varBlock;   
        public ExNamedMem() {   
            InitVar("$var");   
        }   
        public ExNamedMem(string prefix) {   
            InitVar(prefix);   
        }   
        public ExNamedMem(string prefix, string separator) {   
            itemSep = separator; 
            InitVar(prefix);   
        }   
        private void InitVar(string prefix) { 
            prefixStr = prefix; 
            varBlock = BlockHelper.FirstBlockWithPrefix(prefixStr);   
            Set(varBlock.CustomName); 
        } 
        public void WriteToExMem(){  
            varBlock.SetCustomName(Get()); 
        } 
        private void Set(string value){   
            memStr = value;   
            memDic = new Dictionary<string, string>();  
            string[] arr = memStr.Split(new string[]{itemSep}, StringSplitOptions.None);    
            for (int i = 1; i < arr.Length; ++i) {   
                if (arr[i] == "")   
                    continue;   
                string[] nvArr = arr[i].Split(new string[]{nameSep}, StringSplitOptions.None);   
                memDic[nvArr[0]] = nvArr.Length == 1 ? "" : nvArr[1];   
            }  
        }  
        private string Get(){  
            StringBuilder sb = new StringBuilder(); 
            sb.Append(prefixStr); 
            sb.Append(itemSep); 
            string[] keys = new string[memDic.Count];  
            memDic.Keys.CopyTo(keys,0);  
            for (int i = 0; i <  keys.Length; ++i) {  
                sb.Append(keys[i]);  
                sb.Append(nameSep);  
                sb.Append(memDic[keys[i]]);  
                sb.Append(itemSep);  
            }  
            return sb.ToString();  
        }  
        public string GetString(string name){  
            if (!memDic.ContainsKey(name))  
                memDic[name]="";  
            return memDic[name];  
        }   
        public int GetInt(string name){    
            int value = 0;   
            int.TryParse(GetString(name), out value);   
            return value;   
        }  
        public float GetFloat(string name){   
            float value = 0;   
            float.TryParse(GetString(name), out value);  
            return value;  
        }  
        public void SetString(string name, string value){  
            memDic[name] = value;  
            WriteToExMem(); 
        }     
        public void SetVar(string name, object value){    
            SetString(name, Convert.ToString(value)); 
        }  
        public void SetInt(string name, int value){   
            SetString(name,  Convert.ToString(value));   
        }    
        public void SetFloat(string name, float value){ 
            SetString(name,  Convert.ToString(value)); 
        } 
    }
    void InitMem(){ 
        if (Storage == null) 
            Storage = ""; 
        Mem.Init(Storage);
    }  
    void WriteMem(){ 
        Mem.WriteToStr(ref Storage); 
    } 
    void SaveMem(){ 
        WriteMem(); 
    } 
    class Mem{  
        static private string itemSep = "|";  
        static private string nameSep = ":"; 
        static private string arraySep = ","; 
        static private string memStr; 
        static private Dictionary<string, string> memDic; 
        static public void Init(string str){ 
            Set(str); 
        } 
        static public void WriteToStr(ref string str){ 
            str = Get(); 
        } 
        static public void Set(string value){  
            memStr = value;  
            memDic = new Dictionary<string, string>(); 
            string[] arr = memStr.Split(new string[]{itemSep}, StringSplitOptions.None);   
            for (int i = 0; i < arr.Length; ++i) {  
                if (arr[i] == "")  
                    continue;  
                string[] nvArr = arr[i].Split(new string[]{nameSep}, StringSplitOptions.None);  
                memDic[nvArr[0]] = nvArr.Length == 1 ? "" : nvArr[1];  
            } 
        } 
        static public string Get(){ 
            StringBuilder sb = new StringBuilder(); 
            string[] keys = new string[memDic.Count]; 
            memDic.Keys.CopyTo(keys,0); 
            for (int i = 0; i <  keys.Length; ++i) { 
                sb.Append(keys[i]); 
                sb.Append(nameSep); 
                sb.Append(memDic[keys[i]]); 
                sb.Append(itemSep); 
            } 
            return sb.ToString(); 
        } 
        static public string GetString(string name){ 
            if (!memDic.ContainsKey(name)) 
                memDic[name]=""; 
            return memDic[name]; 
        }  
        static public int GetInt(string name){   
            int value = 0;  
            int.TryParse(GetString(name), out value);  
            return value;  
        } 
        static public float GetFloat(string name){  
            float value = 0;  
            float.TryParse(GetString(name), out value); 
            return value; 
        } 
        static public void SetString(string name, string value){ 
            memDic[name] = value; 
        }    
        static public void SetVar(string name, object value){   
            SetString(name,  Convert.ToString(value)); 
        } 
        static public void SetInt(string name, int value){  
            SetString(name,  Convert.ToString(value)); 
        }  
        static public void SetFloat(string name, float value){  
            SetString(name,  Convert.ToString(value)); 
        }  
    } 
