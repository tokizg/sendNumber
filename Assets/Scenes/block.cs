/*using UnityEngine;

public class block : MonoBehaviour
{
    int number;
    public int num
    {
        get { return number; }
    }
    public Vector2Int position;

    //コンストラクタ
    public block( Vector2Int p,int n)
    {
        this.number = n;
        this.position = p;
    }

	public void upGrade()
	{
		this.number *= 2;
	}

	public void move(Vector2Int dir)
	{
		Debug.Log("move"+ dir);
		Vector2Int futurePos = position + dir;
		if (!field.instance.isInside(futurePos)) return;
		int futureBlock = field.instance.getBlock(futurePos);
		if (futureBlock == 0)
		{
        	position = futurePos;
			this.move(dir);
		}
		else if(futureBlock == this.number)
		{
			field.instance.mergeBlock(futurePos);
			field.instance.deleteBlock(this.position);
			delete();
		}
		return;
    }
	public void delete()
	{
		Destroy(gameObject);
	}
}
*/