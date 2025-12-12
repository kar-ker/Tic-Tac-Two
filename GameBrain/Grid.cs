namespace GameBrain;

public class Grid
{
    public int GridSide { get; set; }
    public int GridX { get; set; }
    public int GridY { get; set; }


    public List<Point> Area { get; set; }

    public Grid(int gridSide, int gridX, int gridY)
    {
        GridSide = gridSide;
        GridX = gridX;
        GridY = gridY;
        Area = GetArea(gridSide, new Point(gridX, gridY));
    }

    public bool IsUnderGridArea(int x, int y)
    {
        if (Area.Contains(new Point(x, y)))
        {
            return true;
        }

        return false;
    }
    
    private List<Point> GetArea(int size, Point center)
    {
        List<Point> domain = new List<Point>();
        
        int range = size / 2; 
        if (size % 2 == 0) // for an even side grid
        {
            for (int dy = -range; dy < range; dy++)
            {
                for (int dx = -range; dx < range; dx++)
                {
                    int newX = center.X + dx;
                    int newY = center.Y + dy;
                
                    domain.Add(new Point(newX, newY));
                }
            }
        }
        else
        {
            for (int dy = -range; dy <= range; dy++)
            {
                for (int dx = -range; dx <= range; dx++)
                {
                    int newX = center.X + dx;
                    int newY = center.Y + dy;
                
                    domain.Add(new Point(newX, newY));
                }
            }
        }
        
        return domain;
    }
}