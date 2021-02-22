using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BoxCorp.BusinessLogic
{
    public class BoxSelector
    {
        public static List<Box> SelectBestBoxes(string[] boxLines)
        {
            var boxes = new List<Box>();
            foreach (var theString in boxLines)
            {
                var boxParameters = theString.Split(',');

                //skip header and bad input
                int x, y, width, height;
                decimal rank;
                if ((boxParameters.Length != 5) ||
                    !int.TryParse(boxParameters[0], out x) ||
                    !int.TryParse(boxParameters[1], out y) ||
                    !int.TryParse(boxParameters[2], out width) ||
                    !int.TryParse(boxParameters[3], out height) ||
                    !decimal.TryParse(boxParameters[4], out rank))
                {
                    continue;
                }

                //skip low ranks
                if (rank < (decimal)0.5)
                {
                    continue;
                }

                boxes.Add(new Box()
                {
                    Rectangle = new System.Drawing.Rectangle(x, y, width, height),
                    Rank = rank,
                    IsDeleted = false
                });
            }

            ProcessBoxes(boxes);

            return (boxes.Where(x => !x.IsDeleted)).ToList();
        }

        private static void ProcessBoxes(List<Box> boxes)
        {
            for (int index1 = 0, length = boxes.Count; index1 < length; index1++)
            {
                var box1 = boxes[index1];
                if (box1.IsDeleted)
                {
                    continue;
                }

                for (int index2 = 0; index2 < length; index2++)
                {
                    //skip the same box
                    if (index1 == index2)
                    {
                        continue;
                    }

                    //skip already deleted box
                    var box2 = boxes[index2];
                    if (box2.IsDeleted)
                    {
                        continue;
                    }

                    if (box1.Rectangle.IntersectsWith(box2.Rectangle))
                    {
                        var intersection = Rectangle.Intersect(box1.Rectangle, box2.Rectangle);
                        if (!intersection.IsEmpty)
                        {
                            CheckJaqardForExclusion(intersection, box1, box2);
                        }
                    }
                }
            }
        }

        private static void CheckJaqardForExclusion(Rectangle intersection, Box box1, Box box2)
        {
            int intersectionArea = intersection.Width * intersection.Height;

            double unionArea = (box1.Rectangle.Width * box1.Rectangle.Height) + (box2.Rectangle.Width * box2.Rectangle.Height) - intersectionArea;

            double jaqardIndex = intersectionArea / unionArea;

            if (jaqardIndex > 0.4)
            {
                box1.IsDeleted = (box1.Rank < box2.Rank);
                box2.IsDeleted = !box1.IsDeleted;
            }
        }
    }
}
