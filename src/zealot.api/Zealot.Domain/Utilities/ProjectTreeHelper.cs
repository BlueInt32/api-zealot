using System;
using Zealot.Domain.Enums;
using Zealot.Domain.Exceptions;
using Zealot.Domain.Objects;

namespace Zealot.Domain.Utilities
{
    public static class ProjectTreeHelper<TContext>
    {
        public static void ExecuteTraversal(Node startNode, Action<Node, TContext> nodeAction, TContext context)
        {
            switch (startNode)
            {
                case PackNode packNode:
                    nodeAction(packNode, context);
                    packNode.Children?.ForEach(c => ExecuteTraversal(c, nodeAction, context));
                    break;
                case RequestNode requestNode:
                    nodeAction(requestNode, context);
                    break;
                case ScriptNode scriptNode:
                    nodeAction(scriptNode, context);
                    break;
            }
        }
    }

    public static class ProjectTreeHelperExtensions
    {
        public static string GetTypeConstant(this Node node)
        {
            switch (node)
            {
                case PackNode packNode:
                    return TreeNodeType.Pack.ToString().ToLower();
                case RequestNode requestNode:
                    return TreeNodeType.Request.ToString().ToLower();
                case ScriptNode scriptNode:
                    return TreeNodeType.Script.ToString().ToLower();
                default:
                    throw new ZealotException("UNKNOW_NODE_TYPE");
            }
        }
    }
}
