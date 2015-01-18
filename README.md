# SpaceEngineers Programmable Block Helper
  
  
## 通用物块操作函数  

* `IMyTerminalBlock GetBlock(string name)` 传递物块名，返回物块对象  
* `T GetBlock<T>(string name)` ~~功能同上，泛型版~~现阶段存在问题  
* `void RunAction(string blockName, string actionName)` 传递物块名，执行 Action  
* `void RunAction(IMyTerminalBlock block, string actionName)` 传递物块对象，执行 Action  
* `void BatchRunAction(IList<string> blockNames, string actionName)` 通过物块名列表（如数组），指定多个物块执行 Action  
* `void BatchRunAction(IList<IMyTerminalBlock> blocks, string actionName)` 多个物块执行 Action  
  
  
  
## Var类  

实现了通用的变量存储功能  
_SE 编程部分没有能够存储状态的全局变量之类的东西，所以这里是通过把值存在某个物块的名字里实现的。_  
  
  
### 构造函数  
  
```
Var(IMyGridTerminalSystem gts)  
Var(IMyGridTerminalSystem gts, string prefix)  
Var(IMyGridTerminalSystem gts, string prefix, string separator)  
```
  
**参数说明**
  
 **gts** 传递GridTerminalSystem  
 **prefix** 用来存变量的物块名称前缀，不指定默认是 “$var”  
 _要注意的查找物块时只要包含$var就会返回，也就是说如果你同时有一个叫$var1的物块用来存其他变量，很可能本来你要存在$var上的值会存到$var1上。_  
 **spearator** 变量间的分隔符，不指定默认是 “|”（竖线）  
  
  
### 方法
* `T GetVar<T>()` 返回指定类型的值  
* `T GetVar<T>(uint index)` 同上，不过可以指定索引位置（从0开始），也就是说一个Var对象可以存储多个值  
* `T GetVarWithDefault<T>(object default)` 返回指定类型的值，如果获取失败则返回指定的默认值
* `T GetVarWithDefault<T>(uint index, object default)` 同上，不过可以指定索引位置（从0开始）
* `void SetVar(object value)` 设置值  
* `void SetVar(uint index, object value)` 同上，可以指定索引位置，同样也是0开始  
