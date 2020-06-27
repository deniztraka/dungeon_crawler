using System;
using UnityEngine;
using Random = System.Random;

namespace DTWorldz.ProceduralGeneration
{
    public class BinaryTree
    {
        private Random random;
        public BinaryTree(Random random)
        {
            this.random = random;
        }
        public BinaryTreeNode Root { get; set; }

        public BinaryTreeNode FindMin(BinaryTreeNode node)
        {
            BinaryTreeNode current = node;

            /* loop down to find the leftmost leaf */
            while (current.LeftNode != null)
            {
                current = current.LeftNode;
            }
            return (current);
        }

        public BinaryTreeNode FindMax(BinaryTreeNode node)
        {
            BinaryTreeNode current = node;

            /* loop down to find the rightmost leaf */
            while (current.RightNode != null)
            {
                current = current.RightNode;
            }
            return (current);
        }

        public bool Add(Room room)
        {
            BinaryTreeNode before = null, after = this.Root;
            var newRoomSurfaceArea = room.GetSurcafeArea();
            while (after != null)
            {
                before = after;

                if (newRoomSurfaceArea < after.Room.GetSurcafeArea()) //Is new node in left tree? 
                    after = after.LeftNode;
                else if (newRoomSurfaceArea > after.Room.GetSurcafeArea()) //Is new node in right tree?
                    after = after.RightNode;
                else
                {
                    //Exist same value
                    return false;
                }
            }

            BinaryTreeNode newNode = new BinaryTreeNode(room, random);

            if (this.Root == null)//Tree ise empty
                this.Root = newNode;
            else
            {
                if (newRoomSurfaceArea < before.Room.GetSurcafeArea())
                    before.LeftNode = newNode;
                else
                    before.RightNode = newNode;
            }

            return true;
        }

        public BinaryTreeNode Find(int value)
        {
            return this.Find(value, this.Root);
        }

        public void Remove(int value)
        {
            this.Root = Remove(this.Root, value);
        }

        private BinaryTreeNode Remove(BinaryTreeNode parent, int surfaceArea)
        {
            if (parent == null) return parent;

            if (surfaceArea < parent.Room.GetSurcafeArea()) parent.LeftNode = Remove(parent.LeftNode, surfaceArea);
            else if (surfaceArea > parent.Room.GetSurcafeArea())
                parent.RightNode = Remove(parent.RightNode, surfaceArea);

            // if value is same as parent's value, then this is the node to be deleted  
            else
            {
                // node with only one child or no child  
                if (parent.LeftNode == null)
                    return parent.RightNode;
                else if (parent.RightNode == null)
                    return parent.LeftNode;

                // node with two children: Get the inorder successor (smallest in the right subtree)  
                parent.Room = MinValue(parent.RightNode);

                // Delete the inorder successor  
                parent.RightNode = Remove(parent.RightNode, parent.Room.GetSurcafeArea());
            }

            return parent;
        }

        private Room MinValue(BinaryTreeNode node)
        {
            var room = node.Room;

            while (node.LeftNode != null)
            {
                room = node.LeftNode.Room;
                node = node.LeftNode;
            }

            return room;
        }

        private BinaryTreeNode Find(int value, BinaryTreeNode parent)
        {
            if (parent != null)
            {
                if (value == parent.Room.GetSurcafeArea()) return parent;
                if (value < parent.Room.GetSurcafeArea())
                    return Find(value, parent.LeftNode);
                else
                    return Find(value, parent.RightNode);
            }

            return null;
        }

        public int GetTreeDepth()
        {
            return this.GetTreeDepth(this.Root);
        }

        private int GetTreeDepth(BinaryTreeNode parent)
        {
            return parent == null ? 0 : Math.Max(GetTreeDepth(parent.LeftNode), GetTreeDepth(parent.RightNode)) + 1;
        }

        public void TraversePreOrder(BinaryTreeNode parent)
        {
            if (parent != null)
            {
                Debug.Log(parent.Room.GetSurcafeArea() + " ");
                TraversePreOrder(parent.LeftNode);
                TraversePreOrder(parent.RightNode);
            }
        }

        public void TraverseInOrder(BinaryTreeNode parent)
        {
            if (parent != null)
            {
                TraverseInOrder(parent.LeftNode);
                Debug.Log(parent.Room.GetSurcafeArea() + " ");
                TraverseInOrder(parent.RightNode);
            }
        }

        internal int GetNodeDepth(BinaryTreeNode node)
        {
            return this.GetTreeDepth(node);
        }

        public void TraversePostOrder(BinaryTreeNode parent)
        {
            if (parent != null)
            {
                TraversePostOrder(parent.LeftNode);
                TraversePostOrder(parent.RightNode);
                Debug.Log(parent.Room.GetSurcafeArea() + " ");
            }
        }
    }
}