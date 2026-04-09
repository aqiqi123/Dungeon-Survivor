using System.Collections.Generic;
using UnityEngine;

public class QuadTreeNode
{
    private readonly Rect bounds;                   //此节点的区域范围
    private readonly int capacity;                  //此节点最多存放多少个敌人
    private readonly int maxDepth;                  //树的最大深度（防止无限分割）
    private readonly int depth;                     //当前节点所在深度

    private readonly List<EnemyAgent> objects;      //存放在此节点的敌人
    private QuadTreeNode[] children;                //四个子节点

    //初始化节点
    public QuadTreeNode(Rect bounds, int capacity,int maxDepth,int depth = 0)
    {
        this.bounds = bounds;
        this.capacity = capacity;
        this.maxDepth = maxDepth;
        this.depth = depth;
        this.objects = new List<EnemyAgent>(capacity);
    }

    //插入敌人
    public bool Insert(EnemyAgent enemy)
    {
        //敌人不在范围内
        if (!bounds.Contains(enemy.Position))
            return false;

        //当前节点空间足够，或者当前节点已经是最深层
        if (objects.Count < capacity || depth >= maxDepth)
        {
            objects.Add(enemy);
            return true;
        }

        //如果当前节点空间满了，确保当前节点已分割
        if (children == null)
            Subdivide();

        for(int i = 0; i < 4; i++)
        {
            if (children[i].Insert(enemy))
                return true;
        }

        //边界重叠的极端情况
        objects.Add(enemy);
        return true;
    }

    //查询某个范围内的所有敌人
    public void Query(Rect range,List<EnemyAgent> result) //收集器模式
    {
        if (!bounds.Overlaps(range))
            return;

        for(int i = 0; i < objects.Count; i++)
        {
            if (range.Contains(objects[i].Position))
                result.Add(objects[i]);
        }

        if (children == null)
            return;
        for(int i = 0; i < 4; i++)
        {
            children[i].Query(range, result);
        }
    }

    //清空
    public void Clear()
    {
        objects.Clear();

        if (children == null)
            return;

        for(int i = 0; i < children.Length; i++)
        {
            children[i]?.Clear();
        }
    }

    //空间分割
    private void Subdivide()
    {
        children = new QuadTreeNode[4];

        float halfW = bounds.width * 0.5f;
        float halfH = bounds.height * 0.5f;
        float x = bounds.xMin;
        float y = bounds.yMin;

        //左上（0）、右上（1）、左下（2）、右下（3）
        children[0] = new QuadTreeNode(new Rect(x, y + halfH, halfW, halfH), capacity, maxDepth, depth + 1);
        children[1] = new QuadTreeNode(new Rect(x + halfW, y + halfH, halfW, halfH), capacity, maxDepth, depth + 1);
        children[2] = new QuadTreeNode(new Rect(x, y, halfW, halfH), capacity, maxDepth, depth + 1);
        children[3] = new QuadTreeNode(new Rect(x + halfW, y, halfW, halfH), capacity, maxDepth, depth + 1);
    }
}
