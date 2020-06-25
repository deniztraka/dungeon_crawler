using UnityEngine;

namespace com.gstudios.mapgen {

    public class Room {
        protected int left, right, top, bottom;

        protected int GetWidth() {
            return right - left + 1;
        }

        protected int GetHeight() {
            return top - bottom + 1;
        }

        public Room(int left, int right, int top, int bottom) {
            this.left = left;
            this.right = right;
            this.top = top;
            this.bottom = bottom;
        }

        public int GetLeft() { return left; }
        public int GetRight() { return right; }
        public int GetTop() { return top; }
        public int GetBottom() { return bottom; }

        public virtual GameObject Draw() {
            GameObject roomContainer = new GameObject("Room");
            Color debugColor = Random.ColorHSV();

            for (int x = left; x <= right; x++) {
                for (int y = bottom; y <= top; y++) {

                    GameObject tile = new GameObject("Tile");
                    tile.transform.position = new Vector3(x, y, 0);
                    tile.transform.localScale = Vector3.one * 6.25f;
                    tile.transform.SetParent(roomContainer.transform, true);

                    SpriteRenderer sr = tile.AddComponent<SpriteRenderer>();
                    sr.sprite = Resources.Load<Sprite>("square");
                    sr.color = debugColor;
                }
            }

            return roomContainer;
        }
    }

}