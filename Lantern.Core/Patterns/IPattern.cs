

using System.Drawing;

namespace Lantern.Core.Patterns
{
    public interface IPattern
    {
        void Update(double delta);

        Color Render(int index);
    }
}