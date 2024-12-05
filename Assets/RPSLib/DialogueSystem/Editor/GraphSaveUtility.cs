using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPSCore {

    public class GraphSaveUtility {

        private List<Edge> Edges => _graphView.edges.ToList();
        private List<DialogueNode> Nodes => _graphView.nodes.ToList().Cast<DialogueNode>().ToList();
        private DialogueContainer _dialogueContainer;
        private StoryGraphView _graphView;

        public static GraphSaveUtility GetInstance(StoryGraphView graphView) {
            return new GraphSaveUtility {
                _graphView = graphView
            };
        }

        public void SaveGraph(string fileName) {
            var dialogueContainerObject = ScriptableObject.CreateInstance<DialogueContainer>();
            if (!SaveNodes(fileName, dialogueContainerObject))
                return;
            SaveExposedProperties(dialogueContainerObject);

            Object loadedAsset = null;

            string[] assetGUIDs = AssetDatabase.FindAssets(fileName);
            if (assetGUIDs != null && assetGUIDs.Length > 0) {
                string path = AssetDatabase.GUIDToAssetPath(assetGUIDs[0]);
                loadedAsset = AssetDatabase.LoadAssetAtPath(path, typeof(DialogueContainer));
            }

            if (loadedAsset == null || !AssetDatabase.Contains(loadedAsset)) {
                AssetDatabase.CreateAsset(dialogueContainerObject, $"Assets/{fileName}.asset");
            } else {
                DialogueContainer container = loadedAsset as DialogueContainer;
                container.NodeLinks = dialogueContainerObject.NodeLinks;
                container.DialogueNodeData = dialogueContainerObject.DialogueNodeData;
                container.ExposedProperties = dialogueContainerObject.ExposedProperties;
                EditorUtility.SetDirty(container);
            }

            AssetDatabase.SaveAssets();
        }

        private bool SaveNodes(string fileName, DialogueContainer dialogueContainerObject) {
            if (!Edges.Any())
                return false;
            var connectedSockets = Edges.Where(x => x.input.node != null).ToArray();
            for (var i = 0; i < connectedSockets.Count(); i++) {
                var outputNode = connectedSockets[i].output.node as DialogueNode;
                var inputNode = connectedSockets[i].input.node as DialogueNode;
                dialogueContainerObject.NodeLinks.Add(new NodeLinkData {
                    BaseNodeGUID = outputNode.GUID,
                    PortName = connectedSockets[i].output.portName,
                    TargetNodeGUID = inputNode.GUID
                });
            }

            foreach (var node in Nodes.Where(node => !node.EntyPoint)) {
                dialogueContainerObject.DialogueNodeData.Add(new DialogueNodeData {
                    NodeGUID = node.GUID,
                    DialogueText = node.DialogueText,
                    Speaker = node.Speaker,
                    Position = node.GetPosition().position
                });
            }

            return true;
        }

        private void SaveExposedProperties(DialogueContainer dialogueContainer) {
            dialogueContainer.ExposedProperties.Clear();
            dialogueContainer.ExposedProperties.AddRange(_graphView.ExposedProperties);
        }

        public void LoadNarrative(string fileName) {
            string[] assetGUIDs = AssetDatabase.FindAssets(fileName);
            if (assetGUIDs != null && assetGUIDs.Length > 0) {
                string path = AssetDatabase.GUIDToAssetPath(assetGUIDs[0]);
                _dialogueContainer = AssetDatabase.LoadAssetAtPath<DialogueContainer>(path);
            }

            //_dialogueContainer = Resources.Load<DialogueContainer>(fileName);

            if (_dialogueContainer == null) {
                EditorUtility.DisplayDialog("File Not Found", "Target Narrative Data does not exist!", "OK");
                return;
            }

            ClearGraph();
            GenerateDialogueNodes();
            ConnectDialogueNodes();
            AddExposedProperties();
        }

        /// <summary>
        /// Set Entry point GUID then Get All Nodes, remove all and their edges. Leave only the entrypoint node. (Remove its edge too)
        /// </summary>
        private void ClearGraph() {
            Nodes.Find(x => x.EntyPoint).GUID = _dialogueContainer.NodeLinks[0].BaseNodeGUID;
            foreach (var perNode in Nodes) {
                if (perNode.EntyPoint)
                    continue;
                Edges.Where(x => x.input.node == perNode).ToList()
                    .ForEach(edge => _graphView.RemoveElement(edge));
                _graphView.RemoveElement(perNode);
            }
        }

        /// <summary>
        /// Create All serialized nodes and assign their guid and dialogue text to them
        /// </summary>
        private void GenerateDialogueNodes() {
            foreach (var perNode in _dialogueContainer.DialogueNodeData) {
                var tempNode = _graphView.CreateNode(perNode.Speaker, perNode.DialogueText, Vector2.zero);
                tempNode.Speaker = perNode.Speaker;
                tempNode.GUID = perNode.NodeGUID;
                _graphView.AddElement(tempNode);

                var nodePorts = _dialogueContainer.NodeLinks.Where(x => x.BaseNodeGUID == perNode.NodeGUID).ToList();
                nodePorts.ForEach(x => _graphView.AddChoicePort(tempNode, x.PortName));
            }
        }

        private void ConnectDialogueNodes() {
            for (var i = 0; i < Nodes.Count; i++) {
                var k = i; //Prevent access to modified closure
                var connections = _dialogueContainer.NodeLinks.Where(x => x.BaseNodeGUID == Nodes[k].GUID).ToList();
                for (var j = 0; j < connections.Count(); j++) {
                    var targetNodeGUID = connections[j].TargetNodeGUID;
                    var targetNode = Nodes.First(x => x.GUID == targetNodeGUID);
                    LinkNodesTogether(Nodes[i].outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0]);

                    targetNode.SetPosition(new Rect(
                        _dialogueContainer.DialogueNodeData.First(x => x.NodeGUID == targetNodeGUID).Position,
                        _graphView.DefaultNodeSize));
                }
            }
        }

        private void LinkNodesTogether(Port outputSocket, Port inputSocket) {
            var tempEdge = new Edge() {
                output = outputSocket,
                input = inputSocket
            };
            tempEdge?.input.Connect(tempEdge);
            tempEdge?.output.Connect(tempEdge);
            _graphView.Add(tempEdge);
        }

        private void AddExposedProperties() {
            _graphView.ClearBlackBoardAndExposedProperties();
            foreach (var exposedProperty in _dialogueContainer.ExposedProperties) {
                _graphView.AddPropertyToBlackBoard(exposedProperty);
            }
        }

    }

}