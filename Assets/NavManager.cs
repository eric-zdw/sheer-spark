using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NavManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(NavigateToObject());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>When called by an enemy, a path is calculated given every interval until path reaches destination</summary>
    public static IEnumerator NavigateToLocation(Vector3 start, Vector3 target, bool isGrounded, List<Node> nodePath) {
        //print("start");
        //print("Navigation restarted");
        List<NodeData> openNodes = new List<NodeData>();
        HashSet<NodeData> closedNodes = new HashSet<NodeData>();

        NodeData startNode = new NodeData(FindClosestNode(start, isGrounded));
        //print("start: " + startNode.node.transform.position);
        startNode.gCost = 0f;
        startNode.hCost = Vector3.Distance(startNode.node.transform.position, target);
        startNode.fCost = startNode.hCost;
        openNodes.Add(startNode);

        Node targetNode = FindClosestNode(target, isGrounded);
        //print("target: " + targetNode.transform.position);

        int stepsCalculated = 0;

        bool targetFound = false;

        NodeData lowestHCostNode = startNode;

        while (openNodes.Count > 0 && !targetFound) {
            //print("start2");
        	NodeData currentNode = openNodes[openNodes.Count - 1];
        	closedNodes.Add(currentNode);
        	openNodes.RemoveAt(openNodes.Count - 1);

            if (currentNode.node == targetNode) {
                targetFound = true;
                //print("targethit");
                CalculatePath(currentNode, nodePath);
            }
            else {
                //print("neighbournodes: " + currentNode.node.neighbourNodes.Count);
                if (!isGrounded) {
        	        foreach (GameObject neighbour in currentNode.node.neighbourNodes) {
        	        	if (neighbour) {
                            // Node is in closed set -- ignore
        	        		if (FindNodeData(neighbour.GetComponent<Node>(), closedNodes) != null) {
                                //print("neighbour found in closed");
        	        			//do nothing
        	        		}
                            // Node is not in open set -- create new NodeData and calculate score
        	        		else if (FindNodeData(neighbour.GetComponent<Node>(), openNodes) == null) {
                                //print("neighbour not in open");
        	        			NodeData newData = new NodeData(neighbour.GetComponent<Node>());
        	        			newData.parent = currentNode;
        	        			newData.gCost = currentNode.gCost + Vector3.Distance(neighbour.transform.position, currentNode.node.transform.position);
        	        			newData.hCost = Vector3.Distance(neighbour.transform.position, target);
        	        			newData.fCost = newData.gCost + newData.hCost;
                                openNodes.Add(newData);

                                if (newData.hCost < lowestHCostNode.hCost) {
                                    lowestHCostNode = newData;
                                }
        	        		}
                            // Node is in open set -- update existing NodeData
                            else if (FindNodeData(neighbour.GetComponent<Node>(), openNodes) != null) {
                                //print("neighbour in open");
                                NodeData neighbourND = FindNodeData(neighbour.GetComponent<Node>(), openNodes);
                                float newGCost = currentNode.gCost + Vector3.Distance(neighbour.transform.position, currentNode.node.transform.position);
                                if (newGCost < neighbourND.gCost) {
                                    neighbourND.parent = currentNode;
                                    neighbourND.gCost = newGCost;
                                    neighbourND.fCost = neighbourND.gCost + neighbourND.hCost;

                                    if (neighbourND.hCost < lowestHCostNode.hCost) {
                                        lowestHCostNode = neighbourND;
                                    }
                                }
                            }
                            // Sort open nodes based on fCost, best node last
                            openNodes = openNodes.OrderByDescending(n=>n.fCost).ToList();

                            stepsCalculated++;
        	        	}
        	        }
                }
                else if (isGrounded) {
                    // Populate candidates for cost calculation.
                    List<Node> candidates = new List<Node>();
                    foreach (GameObject neighbour in currentNode.node.neighbourNodes) {
                        if (neighbour && neighbour.GetComponent<Node>().isGroundNode) {
                            candidates.Add(neighbour.GetComponent<Node>());
                        }
                    }
                    candidates.AddRange(currentNode.node.dropConnections);
                    candidates.AddRange(currentNode.node.jumpConnections);

        	        foreach (Node candidate in candidates) {
        	        	if (FindNodeData(candidate, closedNodes) != null) {
                            //print("neighbour found in closed");
        	        		//do nothing
        	        	}

                        // Node is not in open set -- create new NodeData and calculate score
        	        	else if (FindNodeData(candidate, openNodes) == null) {
                            //print("neighbour not in open");
        	        		NodeData newData = new NodeData(candidate);
        	        		newData.parent = currentNode;
        	        		newData.gCost = currentNode.gCost + Vector3.Distance(candidate.transform.position, currentNode.node.transform.position);
        	        		newData.hCost = Vector3.Distance(candidate.transform.position, target);
        	        		newData.fCost = newData.gCost + newData.hCost;
                            openNodes.Add(newData);
                            if (newData.hCost < lowestHCostNode.hCost) {
                                lowestHCostNode = newData;
                            }
        	        	}

                        // Node is in open set -- update existing NodeData
                        else if (FindNodeData(candidate, openNodes) != null) {
                            //print("neighbour in open");
                            NodeData neighbourND = FindNodeData(candidate, openNodes);
                            float newGCost = currentNode.gCost + Vector3.Distance(candidate.transform.position, currentNode.node.transform.position);
                            if (newGCost < neighbourND.gCost) {
                                neighbourND.parent = currentNode;
                                neighbourND.gCost = newGCost;
                                neighbourND.fCost = neighbourND.gCost + neighbourND.hCost;
                                if (neighbourND.hCost < lowestHCostNode.hCost) {
                                    lowestHCostNode = neighbourND;
                                }
                            }
                        }

                        // Sort open nodes based on fCost, best node last
                        openNodes = openNodes.OrderByDescending(n=>n.fCost).ToList();
                        
                        stepsCalculated++;
        	        }
                }

                if (stepsCalculated >= 50) {
                    CalculatePath(lowestHCostNode, nodePath);
                    //print("NavManager nodepath size: " + nodePath.Count);
                    yield return new WaitForSeconds(UnityEngine.Random.Range(0.375f, 0.625f));
                    stepsCalculated = 0;
                }
            }
        }
    }

    // Calculate additions to currentPath given destination.
    // If CalculatePath generates a duplicate node from currentPath, the new path is
    // appended to the path and any existing parents of that node are cleared.
    // If no matching nodes are found, a new path was likely found.
    private static void CalculatePath(NodeData dest, List<Node> currentPath) {
        List<Node> path = new List<Node>();
        NodeData currentND = dest;
         
        path.Insert(0, currentND.node);
        bool existingNodeFound = false;
        while (currentND.parent != null && !existingNodeFound) {
            // check if current node is already in the navigation path
            if (currentPath.Contains(currentND.node)) {
                int index = currentPath.FindIndex(n => n == currentND.node);
                if (index != currentPath.Count - 1) {
                    //print("removing " + (currentPath.Count - index) + " elements at index " + index);
                    currentPath.RemoveRange(index, currentPath.Count - index);
                }
                existingNodeFound = true;
            }
            else {
                currentND = currentND.parent;
                path.Insert(0, currentND.node);
            }
        }

        currentPath.AddRange(path);
    }


    // Find the node closest to the given position.
    private static Node FindClosestNode(Vector3 pos, bool isGrounded) {

        float smallestDistance = Mathf.Infinity;
        Node n = null;

        foreach (List<GameObject> row in NavMeshGenerator.nodes) {
            foreach (GameObject node in row) {
                if (node && (!isGrounded || (isGrounded && node.GetComponent<Node>().isGroundNode))) {
                    float dist = Vector3.Distance(node.transform.position, pos);
                    if (dist < smallestDistance) {
                        smallestDistance = dist;
                        n = node.GetComponent<Node>();
                    }
                }
            }
        }
        
        return n;
    }

    // Attempt to return matching NodeData given Node and IEnumerable; return null if not found
    private static NodeData FindNodeData(Node n, IEnumerable<NodeData> ic) {
        foreach (NodeData nc in ic) {
            if (nc.node == n) {
                return nc;
            }
        }

        return null;
    }
}

public class NodeData {
    public Node node;
	public NodeData parent;
	public float gCost;
	public float hCost;
	public float fCost;

	public NodeData(Node n) {
        this.node = n;
		this.parent = null;
		this.gCost = Mathf.Infinity;
		this.hCost = Mathf.Infinity;
		this.fCost = Mathf.Infinity;
	}

	public NodeData(Node n, NodeData p, float g, float h) {
        this.node = n;
		this.parent = p;
		this.gCost = g;
		this.hCost = h;
		this.fCost = g + h;
	}
}