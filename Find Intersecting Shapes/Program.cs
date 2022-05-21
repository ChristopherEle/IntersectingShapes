using System.Collections.Generic;
using System;

public class Program
{
    public class Shape
    {
        public int id { get; }
        public float x { get; set; }
        public float y { get; set; }

        public Shape(int newId, float newX, float newY)
        {
            id = newId;
            x = newX;
            y = newY;
        }
    }

    public class Circle : Shape
    {
        public float radius { get; }

        public Circle(int id, float x, float y, float newRadius) : base(id, x, y)
        {
            radius = newRadius;
        }
    }

    public class Rectangle : Shape
    {
        public float width { get; }
        public float height { get; }

        public Rectangle(int id, float x, float y, float newWidth, float newHeight) : base(id, x, y)
        {
            width = newWidth;
            height = newHeight;
        }
    }
    public static List<Shape> allShapes { get; set; } = new List<Shape>();

    public static void CreateShapes()
    {
        allShapes.Add(new Rectangle(1, 20, 0, 10, 10));
        allShapes.Add(new Rectangle(2, 15, 15, 10, 14));
        allShapes.Add(new Circle(3, 15, 5, 5));
        allShapes.Add(new Circle(4, 7, 0, 5));
    }

    public class LookForOverlap
    {
        public static Dictionary<int, List<int>> intersectingShapesD = new Dictionary<int, List<int>>();

        public static Dictionary<int, List<int>> FindIntersections(List<Shape> shapes)
        {
            Console.WriteLine("Intersecting Shapes");

            Dictionary<int, List<int>> intersectingShapes = new Dictionary<int, List<int>>();

            int max = shapes.Count;

            for (int iprimaryShapeIndex = 0; iprimaryShapeIndex < max; iprimaryShapeIndex++)
            {
                Shape primaryShape = shapes[iprimaryShapeIndex];

                for (int iSecondaryShapeIndex = 0; iSecondaryShapeIndex < shapes.Count; iSecondaryShapeIndex++)
                {
                    if (iprimaryShapeIndex != iSecondaryShapeIndex)
                    {
                        Shape secondaryShape = shapes[iSecondaryShapeIndex];

                        if (CompareShapes(primaryShape, secondaryShape))
                        {
                            AddIntersectingShapes(primaryShape.id, secondaryShape.id);
                        }
                    }
                }
                ViewIntersectingShapes(primaryShape.id);
            }
            return intersectingShapes;
        }

        public static void ViewIntersectingShapes(int primaryShapeId)
        {
            if (intersectingShapesD.TryGetValue(primaryShapeId, out _))
            {
                foreach (int shape in intersectingShapesD[primaryShapeId])
                {
                    Console.WriteLine("\n" + primaryShapeId + "->" + "(" + shape + ")");
                }
            }
        }

        public static bool CompareShapes(Shape shapeA, Shape shapeB)
        {
            switch (shapeA)
            {
                case Rectangle rectangleA:
                    {
                        switch (shapeB)
                        {
                            case Rectangle rectangleB:
                                {
                                    return (CheckIfIntersecting(rectangleA, rectangleB));
                                }

                            case Circle circleB:
                                {
                                    return (CheckIfIntersecting(rectangleA, circleB));
                                }
                        }

                        break;
                    }

                case Circle circleA:
                    {
                        switch (shapeB)
                        {
                            case Circle circleB:
                                {
                                    return (CheckIfIntersecting(circleA, circleB));
                                }

                            case Rectangle rectangleB:
                                {
                                    return (CheckIfIntersecting(rectangleB, circleA));
                                }
                        }

                        break;
                    }
            }

            return false;
        }

        public static bool CheckIfIntersecting(Rectangle rectangleA, Rectangle rectangleB)
        {
            return (rectangleA.x < rectangleB.x + rectangleB.width && rectangleA.x + rectangleA.width > rectangleB.x &&
                 rectangleA.y < rectangleB.y + rectangleB.height && rectangleA.y + rectangleA.height > rectangleB.y);
        }

        public static bool CheckIfIntersecting(Rectangle rectangle, Circle circle)
        {
            float distx;
            float disty;

            distx = Math.Abs(circle.x - rectangle.x);
            disty = Math.Abs(circle.y - rectangle.y);

            if (distx > (rectangle.width / 2 + circle.radius)) { return false; }
            if (disty > (rectangle.height / 2 + circle.radius)) { return false; }

            if (distx <= (rectangle.width / 2)) { return true; }
            if (disty <= (rectangle.height / 2)) { return true; }

            double cornerDistance_sq = Math.Pow(distx - rectangle.width / 2, 2) +
                                 Math.Pow(disty - rectangle.height / 2, 2);

            return (cornerDistance_sq <= Math.Pow(circle.radius, 2));
        }

        public static bool CheckIfIntersecting(Circle circleA, Circle circleB)
        {
            float radius = circleA.radius + circleB.radius;
            float distX = circleA.x - circleB.x;
            float distY = circleA.y - circleB.y;

            return distX * distX + distY * distY <= radius * radius;
        }

        public static void AddIntersectingShapes(int primaryShapeId, int secondaryShapeId)
        {
            if (!intersectingShapesD.TryGetValue(primaryShapeId, out List<int> interSectingIds))
            {
                interSectingIds = new List<int>();
                interSectingIds.Add(secondaryShapeId);
                intersectingShapesD.Add(primaryShapeId, interSectingIds);
                intersectingShapesD[primaryShapeId] = interSectingIds;
            }
            else
            {
                interSectingIds.Add(secondaryShapeId);
                intersectingShapesD[primaryShapeId] = interSectingIds;
            }
        }
    }

    public static void Main(String[] args)
    {
        CreateShapes();

        LookForOverlap.FindIntersections(allShapes);
    }
}