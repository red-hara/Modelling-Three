using System;

public struct Generalized4
{
    public float a;
    public float b;
    public float c;
    public float d;

    public Generalized4(float a, float b, float c, float d)
    {
        this.a = a;
        this.b = b;
        this.c = c;
        this.d = d;
    }

    public static Generalized4 operator *(float scale, Generalized4 generalized)
    {
        return new Generalized4(
            scale * generalized.a,
            scale * generalized.b,
            scale * generalized.c,
            scale * generalized.d
        );
    }

    public static Generalized4 operator *(Generalized4 generalized, float scale)
    {
        return scale * generalized;
    }

    public static Generalized4 operator +(Generalized4 self, Generalized4 other)
    {
        return new Generalized4(
            self.a + other.a,
            self.b + other.b,
            self.c + other.c,
            self.d + other.d
        );
    }

    public static Generalized4 operator -(Generalized4 self, Generalized4 other)
    {
        return new Generalized4(
            self.a - other.a,
            self.b - other.b,
            self.c - other.c,
            self.d - other.d
        );
    }

    public float this[int index]
    {
        get
        {
            switch (index)
            {
                case 0:
                    return a;
                case 1:
                    return b;
                case 2:
                    return c;
                case 3:
                    return d;
            }
            throw new IndexOutOfRangeException();
        }
        set
        {
            switch (index)
            {
                case 0:
                    a = value;
                    break;
                case 1:
                    b = value;
                    break;
                case 2:
                    c = value;
                    break;
                case 3:
                    d = value;
                    break;
                default:
                    throw new IndexOutOfRangeException();
            }
        }
    }
}