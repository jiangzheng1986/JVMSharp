class TestInt
{
	public int i;
	TestInt(int i)
	{
		this.i = i;
	}
}

class ArrayTest
{
	public static void main()
	{
		/*
		int[] arr = new int[2];
		arr[0] = 10;
		arr[1] = 20;
		Console.println(arr[0]);
		Console.println(arr[1]);

		TestInt[] arr2 = new TestInt[2];
		arr2[0] = new TestInt(10);
		arr2[1] = new TestInt(20);
		Console.println(arr2[0].i);
		Console.println(arr2[1].i);
		*/
		int[][] arr = new int[2][2];
		arr[0][0] = 10;
		arr[0][1] = 20;
		Console.println(arr[0][0]);
		Console.println(arr[0][1]);
	}
}