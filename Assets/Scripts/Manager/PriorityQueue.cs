using System.Collections.Generic;

// Priority float형으로 변경
// Priority 값이 작을 수록 우선순위를 가지도록 변경
// Dequeue 함수 반환형을 bool값으로 변경, data는 out으로 반환
// 최우선순위 Queue의 Priority 값만 반환하는 PeekPriority 함수 추가
public class PriorityQueue<T>
{
    private class Node
    {
        public T Data { get; private set; }
        public float Priority { get; set; } = 0;

        public Node(T data, float priority)
        {
            this.Data = data;
            this.Priority = priority;
        }
    }
    
    private List<Node> nodes = new List<Node>();

    public int Count => nodes.Count;

    public void Enqueue(T data, float priority)
    {
        Node newNode = new Node(data, priority);
        if (nodes.Count == 0)
        {
            nodes.Add(newNode);
        }
        else
        {
            //////////////////////////////
            // 이진 탐색을 시작한다. 'O(logN)'
            int start = 0;
            int end = nodes.Count - 1;
            int harf = 0;
            while (start != end)
            {
                if (end - start == 1)
                {
                    if (nodes[start].Priority > priority)
                    {
                        harf = end;
                    }
                    else
                    {
                        harf = start;
                    }
                    break;
                }
                else
                {
                    harf = start + ((end - start) / 2);
                    if (nodes[harf].Priority < priority)
                    {
                        // Down
                        end = harf;
                    }
                    else
                    {
                        // Up
                        start = harf;
                    }
                }
            }
            //////////////////////////////

            if (nodes[harf].Priority < priority)
                nodes.Insert(harf, newNode);
            else
                nodes.Insert(harf + 1, newNode);
        }
    }

    public bool Dequeue(out T data)
    {
        Node tail = null;

        if (Count > 0)
        {
            tail = nodes[nodes.Count - 1];
            nodes.RemoveAt(nodes.Count - 1);
        }

        if (tail != null)
        {
            data = tail.Data;
            return true;
        }
        else
        {
            data = default(T);
            return false;
        }
    }

    public T Peek()
    {
        Node tail = null;

        if (Count > 0)
            tail = nodes[nodes.Count - 1];

        if (tail != null)
        {
            return tail.Data;
        }
        else
        {
            return default(T);
        }
    }

    public float PeekPriority()
    {
        Node tail = null;

        if (Count > 0)
            tail = nodes[nodes.Count - 1];

        if (tail != null)
        {
            return tail.Priority;
        }
        else
        {
            return float.MaxValue;
        }
    }
}
