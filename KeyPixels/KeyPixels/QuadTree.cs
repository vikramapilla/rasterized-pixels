using Microsoft.Xna.Framework;
using System.Collections.Generic;


namespace KeyPixels
{
    class QuadTree<T>
    {
        public struct Node
        {
            public Vector2 min;
            public Vector2 max;
            public List<Node> child;
            public List<T> data;
        };

        private Node root;
        private float minDelta;
        private static bool insert;

        public QuadTree(Vector2 _min, Vector2 _max, float _minDelta)
        {
            root = new Node();
            root.max = _max;
            root.min = _min;
            root.child = new List<Node>(4);
            root.data = new List<T>();
            minDelta = _minDelta;
        }

        public List<T> seekData(Vector2 _min, Vector2 _max)
        {
            List<T> ret_temp = new List<T>();
            Stack<Vector2> _stack = new Stack<Vector2>();
            _stack.Push(_min);
            _stack.Push(_max);
            _stack.Push(new Vector2(_min.X, _max.Y));
            _stack.Push(new Vector2(_max.X, _min.Y));

            while (_stack.Count > 0)
            {
                List<T> temp = _seekData(root, _stack.Pop());
                bool seek = true;
                for (int i = 0; i < temp.Count; ++i)
                {
                    for (int x = 0; x < ret_temp.Count && seek; ++x)
                    {
                        if (ret_temp[x].ToString() == temp[i].ToString())
                            seek = false;
                    }
                    if (seek)
                        ret_temp.Add(temp[i]);
                    seek = true;
                }
            }
            return ret_temp;
        }

        private List<T> _seekData(Node temp, Vector2 vec)
        {
            List<T> ret_temp = new List<T>();
            for (int i = 0; i < temp.child.Count; ++i)
            {
                if (temp.child[i].min.X < vec.X && temp.child[i].min.Y < vec.Y &&
                    temp.child[i].max.X > vec.X && temp.child[i].max.Y > vec.Y)
                        ret_temp.AddRange(_seekData(temp.child[i], vec));
                
            }
            ret_temp.AddRange(temp.data);
            return ret_temp;
        }

        public List<T> seekAllData()
        {
            return _seekAllData(root);
        }

        private List<T> _seekAllData(Node temp)
        {
            List<T> ret_temp = new List<T>();
            for (int i = 0; i < temp.child.Count; ++i)
                ret_temp.AddRange(_seekAllData(temp.child[i]));

            ret_temp.AddRange(temp.data);
            return ret_temp;
        }

        public void insertData(T _data, Vector2 _min, Vector2 _max)
        {
            insert = false;
            _insertData(ref root, _data, _min, _max, minDelta);
        }

        private void _insertData(ref Node temp, T _data, Vector2 _min, Vector2 _max, float _minDelta)
        {
            int i = 0;
            float midlX = temp.min.X + (MathHelper.Distance(temp.min.X, temp.max.X) / 2);
            float midlY = temp.min.Y + (MathHelper.Distance(temp.min.Y, temp.max.Y) / 2);
            if ((_min.X > midlX || _max.X < midlX) && (_min.Y > midlY || _max.Y < midlY))
            {
                for (; i < temp.child.Count; ++i)
                {
                    if (temp.child[i].min.X < _min.X && temp.child[i].min.Y < _min.Y &&
                        temp.child[i].max.X > _max.X && temp.child[i].max.Y > _max.Y)
                    {
                        var help = temp.child[i];
                        _insertData(ref help, _data, _min, _max, _minDelta);
                        temp.child[i] = help;
                    }
                }
                if ((temp.max.X - temp.min.X) * 2 > _minDelta && (temp.max.Y - temp.min.Y) * 2 > _minDelta && !insert)
                {
                    if (i < 4)
                    {
                        temp.child.Add(new Node());
                        float minX = temp.min.X+ (MathHelper.Distance(temp.min.X, temp.max.X)/2);
                        float maxX = temp.max.X;
                        if (minX > _max.X)
                        {
                            maxX = minX;
                            minX = temp.min.X;
                        }
                        float minY = temp.min.Y + (MathHelper.Distance(temp.min.Y, temp.max.Y)/2);
                        float maxY = temp.max.Y;
                        if (minY > _max.Y)
                        {
                            maxY = minY;
                            minY = temp.min.Y;
                        }
                        var help = temp.child[i];
                        help.min = new Vector2(minX, minY);
                        help.max = new Vector2(maxX, maxY);
                        help.child = new List<Node>();
                        help.data = new List<T>();

                        _insertData(ref help, _data, _min, _max, _minDelta);
                        temp.child[i] = help;
                    }
                }
            }
            if (!temp.data.Equals(_data) && !insert)
            {
                insert = true;
                temp.data.Add(_data);
            }
            else
                insert = true;
        }
    }
}
