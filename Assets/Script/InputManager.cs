using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Vector2 touchStartPosition, touchEndPosition;
    private DirectionType directionType = DirectionType.Still;
    private RaycastHit hit;
    private Camera _camera;
    [SerializeField] private float dpadSense = 0.2f;
    [SerializeField] private float raycastDistance;
    [SerializeField] private LayerMask TileLayer;

    private void Awake()
    {
        _camera = FindObjectOfType<Camera>();
    }
    private void Update()
    {
        //no touch, serve per reseettare il tile data preso
        if (Input.touchCount <= 0)
        {
            GameManager.instance.SelectedTile = null;
            return;
        }

        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            touchStartPosition = touch.position;
            Ray ray = _camera.ScreenPointToRay(touchStartPosition);
            Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.red, 1.0f);

            if (Physics.Raycast(ray, out hit, raycastDistance, TileLayer))
            {
                Tile tile = hit.collider.GetComponent<Tile>();
                GameManager.instance.SelectedTile = tile;
            }
        }
        else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Ended)
        {
            touchEndPosition = touch.position;

            float x = touchEndPosition.x - touchStartPosition.x;
            float y = touchEndPosition.y - touchStartPosition.y;

            if (Mathf.Abs(x) <= dpadSense && Mathf.Abs(y) <= dpadSense)
            {
                directionType = DirectionType.Still;
            }
            else if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                directionType = x > 0 ? DirectionType.Right : DirectionType.Left;
            }
            else
            {
                directionType = y > 0 ? DirectionType.Up : DirectionType.Down;
            }
            DirectionInput();
        }
    }

    //Da spostare altrove o usare in maniera diversa
    void DirectionInput()
    {
        if (GameManager.instance.SelectedTile == null) return;
        else if (GameManager.instance.SelectedTile != null && GameManager.instance.SelectedTile.isFill)
        {
            Vector2Int SelectedPosition = GameManager.instance.SelectedTile.data.ToVector();
            Tile SelectedTile = GameManager.instance.SelectedTile;
            GridManager gridManager = GameManager.instance.gridManager;
            switch (directionType)
            {
                case DirectionType.Still:
                    break;
                case DirectionType.Right:
                    if (gridManager.GetTileWithPosition(SelectedPosition + new Vector2Int(1, 0)) != null)
                    {
                        Tile destinationTile = gridManager.GetTileWithPosition(SelectedPosition + new Vector2Int(1, 0));
                        if (destinationTile.isFill)
                            SelectedTile.TransferIngredients(destinationTile);
                    }
                    GameManager.instance.SelectedTile = null;
                    break;
                case DirectionType.Left:
                    if (gridManager.GetTileWithPosition(SelectedPosition + new Vector2Int(-1, 0)) != null)
                    {
                        Tile destinationTile = gridManager.GetTileWithPosition(SelectedPosition + new Vector2Int(-1, 0));
                        if (destinationTile.isFill)
                            SelectedTile.TransferIngredients(destinationTile);
                    }
                    GameManager.instance.SelectedTile = null;
                    break;
                case DirectionType.Up:
                    if (gridManager.GetTileWithPosition(SelectedPosition + new Vector2Int(0, 1)) != null)
                    {
                        Tile destinationTile = gridManager.GetTileWithPosition(SelectedPosition + new Vector2Int(0, 1));
                        if (destinationTile.isFill)
                            SelectedTile.TransferIngredients(destinationTile);
                    }
                    GameManager.instance.SelectedTile = null;
                    break;
                case DirectionType.Down:
                    if (gridManager.GetTileWithPosition(SelectedPosition + new Vector2Int(0, -1)) != null)
                    {
                        Tile destinationTile = gridManager.GetTileWithPosition(SelectedPosition + new Vector2Int(0, -1));
                        if (destinationTile.isFill)
                            SelectedTile.TransferIngredients(destinationTile);
                    }
                    GameManager.instance.SelectedTile = null;
                    break;
                default:
                    break;
            }
        }
        else return;
    }
}
public enum DirectionType
{
    Still,
    Right,
    Left,
    Up,
    Down
}