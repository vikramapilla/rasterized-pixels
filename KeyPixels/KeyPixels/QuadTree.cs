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
        private static float deltaX, deltaY;

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
            deltaX = MathHelper.Distance(root.min.X, root.max.X);
            deltaY = MathHelper.Distance(root.min.Y, root.max.Y);
            return _seekData(root,_min , _max);
        }

        private List<T> _seekData(Node temp, Vector2 _min, Vector2 _max)
        {
            List<T> ret_temp = new List<T>();
            if (_min.X < temp.min.X && _min.Y < temp.min.Y && _max.X > temp.max.X && _max.Y > temp.max.Y)
                return _seekAllData(temp);
            else
            {
                BoundingBox seekBox = new BoundingBox();
                seekBox.Min = new Vector3(_min.X, _min.Y, 0);
                seekBox.Max = new Vector3(_max.X, _max.Y, 0);

                for (int i = 0; i < temp.child.Count; ++i)
                {
                    BoundingBox helpBox = new BoundingBox();
                    helpBox.Min = new Vector3(temp.child[i].min.X, temp.child[i].min.Y, 0);
                    helpBox.Max = new Vector3(temp.child[i].max.X, temp.child[i].max.Y, 0);

                    if (helpBox.Intersects(seekBox))
                    {
                        deltaX = MathHelper.Distance(temp.child[i].min.X, temp.child[i].max.X);
                        deltaY = MathHelper.Distance(temp.child[i].min.Y, temp.child[i].max.Y);
                            ret_temp.AddRange(_seekData(temp.child[i], _min, _max));
                    }
                }

                Vector2 midl = new Vector2(temp.min.X + (MathHelper.Distance(temp.min.X, temp.max.X) / 2),
                    temp.min.Y + (MathHelper.Distance(temp.min.Y, temp.max.Y) / 2));
                if ((_min.X + deltaX > midl.X && _min.X - deltaX < midl.X) || (_min.Y + deltaY > midl.Y && _min.Y - deltaY < midl.Y))
                    ret_temp.AddRange(temp.data);

                return ret_temp;
            }
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
                        float minX = temp.min.X + (MathHelper.Distance(temp.min.X, temp.max.X) / 2);
                        float maxX = temp.max.X;
                        if (minX > _max.X)
                        {
                            maxX = minX;
                            minX = temp.min.X;
                        }
                        float minY = temp.min.Y + (MathHelper.Distance(temp.min.Y, temp.max.Y) / 2);
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
