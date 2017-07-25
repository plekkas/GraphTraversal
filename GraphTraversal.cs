using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public static class GraphTraversal
{
    private static void BFSDistance(List<Node> nodes, Node start)
    {
        Queue<Node> q = new Queue<Node>();
        List<int> passed = new List<int>();
        q.Enqueue(start);
        while (q.Count > 0)
        {
            Node current = q.Dequeue();
            if (current == null)
                continue;

            passed.Add(current.id);

            List<int> neibs = current.GetNeighboursIds(current.id);

            foreach (int n in neibs)
            {
                Node nNode = nodes.Where(x => x.id == n).SingleOrDefault();
                if (nNode != null)
                {
                    if (!passed.Contains(n))
                    {
                        q.Enqueue(nNode);
                        nNode.distace = current.distace + 1;

                        passed.Add(nNode.id);
                    }

                    nNode.hotScore = current.hotScore;
                    if (nNode.IsHot(nodes))
                        nNode.hotScore++;
                }
            }
        }

        foreach (Node node in nodes)
            node.AddScoreToLinks(nodes);
        foreach (Node node in nodes)
            node.Reset();
    }
}

public class Node
{
    public int id;
    public bool isGateway = false;
    public List<Link> links = new List<Link>();
    public int distace = 0;
    public int hotScore = 0;

    public Node(int _id)
    {
        id = _id;
    }

    public bool IsHot(List<Node> nodes)
    {
        List<Link> activeLinks = links.Where(x => !x.disabled).ToList();
        foreach (Link l in activeLinks)
        {
            if (nodes.Where(x => x.id == l.GetOtherEnd(id)).Single().isGateway)
                return true;
        }
        return false;
    }

    public List<int> GetNeighboursIds(int nId)
    {
        return links.Where(x => !x.disabled).Select(x => x.n1 == nId ? x.n2 : x.n1).ToList();
    }

    public void Reset()
    {
        distace = 0;
        hotScore = 0;
    }
}

public class Link
{
    public int n1;
    public int n2;
    public int score = 0;
    public bool disabled = false;

    public Link(int _n1, int _n2)
    {
        n1 = _n1;
        n2 = _n2;
    }

    public int GetOtherEnd(int thisEnd)
    {
        return thisEnd == n1 ? n2 : n1;
    }
}
