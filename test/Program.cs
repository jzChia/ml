using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MLCommon.Entities;
using Aspose.Cells;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            //List<Factor> fs = new List<Factor>();

            //fs.Add(new Factor { classification = "地层条件", category = "大地构造位置", name = "陆内裂谷" });
            //fs.Add(new Factor { classification = "地层条件", category = "大地构造位置", name = "大陆边缘裂陷槽区" });
            //fs.Add(new Factor { classification = "地层条件", category = "大地构造位置", name = "碰撞后拉展环境" });
            //fs.Add(new Factor { classification = "地层条件", category = "成矿时代", name = "元古宙" });
            //fs.Add(new Factor { classification = "地层条件", category = "成矿时代", name = "晚古生代" });
            //fs.Add(new Factor { classification = "地层条件", category = "成矿时代", name = "三叠纪" });
            //fs.Add(new Factor { classification = "岩体条件", category = "岩石类型", name = "纯橄榄岩、含辉橄榄岩和二辉橄榄岩等组合的超镁铁岩" });

            //fs.Add(new Factor { classification = "构造条件", category = "构造条件", name = "深大断裂" });


            //var res = fs.GroupBy(f => new { f.classification, f.category }).Select(a => a.Key).ToList();
            //foreach (var item in res)
            //{
            //    Console.WriteLine(String.Format("{0}:{1}", item.classification, item.category));
            //}

            //HashSet<String> hs = new HashSet<string>();
            //hs.Add("123");
            //hs.Add("wqe");
            //hs.Add("sfds");
            //hs.Add("cxzc");
            //hs.Add("123");
            //hs.Add("df");
            //hs.Add("12dg3");

           // int[] arr = { 1, 2,2, 4, 5 };

           // List<int> list = new List<int>();
           // list.AddRange(arr);

           // cal(arr, 2);

           // //rotate(arr, list, list.Count);
           // foreach (var item in ls)
           // {
           //     //TODO:dsa
           //     Console.WriteLine(item);
           // }
           //// com(5, 3);

            //Workbook CurrentWorkbook = new Workbook(@"E:\Desktop\新建 Microsoft Excel 工作表 - 副本.xlsx");
            //Factor factor;
            //for (int i = 0; i < CurrentWorkbook.Worksheets.Count; i++)
            //{
            //    string sheetname = CurrentWorkbook.Worksheets[i].Name;
            //    if (sheetname.Equals("Sheet1"))
            //    {
            //        Cells SelectedCells = CurrentWorkbook.Worksheets[sheetname].Cells;
            //        int col = SelectedCells.MaxColumn+1;
            //        int row =SelectedCells.MaxRow+1;
            //        int t = 1;
            //        for (int r = 2; r < row; r++)
            //        {
            //            for (int c = 0; c < col; c++)
            //            {
            //                Cell cellclassification = SelectedCells.CheckCell(0, c);
            //                Cell cellcategory = SelectedCells.CheckCell(1, c);
            //                Cell cell = SelectedCells.CheckCell(r, c);
            //                if (cell != null && cellcategory != null && cellclassification != null)
            //                {
            //                    if (cell.Value == null || cellcategory.Value == null )
            //                    {

            //                    }
            //                    else
            //                    {
            //                        factor = new Factor
            //                        {
            //                            classification = cellcategory.Value.ToString(),
            //                            category = cellcategory.Value.ToString(),
            //                            name = cell.Value.ToString(),
            //                            author = "3s",
            //                            createTime = DateTime.Now,
            //                            updateTime = DateTime.Now

            //                        };
            //                        Console.WriteLine(t+++"--"+cellcategory.Value + ":" + cellcategory.Value + ":" + cell.Value);
                                    
            //                    }
            //                }
            //            }
            //        }
            //    }


           // }

            //HashSet<Factor> set = new HashSet<Factor>();
            //Factor factor = new Factor{ classification = "地层条件", category = "大地构造位置", name = "陆内裂谷" };
            //set.Add(new Factor { classification = "地层条件", category = "大地构造位置", name = "陆内裂谷", referenceMatch = 5 });
            //set.Add(new Factor { classification = "地层条件", category = "大地构造位置", name = "陆内裂谷" });
            //set.Add(new Factor { classification = "地层条件", category = "大地构造位置", name = "大陆边缘裂陷槽区" });
            //set.Add(new Factor { classification = "地层条件", category = "大地构造位置", name = "碰撞后拉展环境" });
            //set.Add(new Factor { classification = "地层条件", category = "成矿时代", name = "元古宙" });
            //set.Add(new Factor { classification = "地层条件", category = "成矿时代", name = "晚古生代" });

            //Console.Write(set.Contains(factor));

            int[] arr = { 1,4,6,7,9,5,1,6,4,1,6,4,7,5,2,6,8,4,3,6,1,2,8,9,4,5,7,5,6,2,0,1,4,5,9,6,4,6,3,4,6,4,6,4,6,3,7,2,8,9,4,1,3,8,6,4};
            List<sortInt> list = new List<sortInt>();
            sortInt si = null;

            
            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write(arr[i] + " ");
                if (list.Find(l => l.no == arr[i]) == null)
                    list.Add(new sortInt(arr[i], 1, i));
                else 
                {
                    si = list.Find(l => l.no == arr[i]);
                    list.Remove(si);
                    si.num = si.num + 1;
                    list.Add(si);
                }
            }
            Console.WriteLine();
            list.Sort(
                delegate(sortInt x, sortInt y)
                {
                    if (x.no == y.no && x.num == y.num)
                        return 0;

                    if (x.num > y.num)
                        return -1;
                    else if (x.num < y.num)
                        return 1;
                    if (x.first < y.first)
                        return -1;
                    else if (x.first > y.first)
                        return 1;

                    return 0;
                });

            list.Sort(
                (x, y) =>
                {
                    if (x.no == y.no && x.num == y.num)
                        return 0;

                    if (x.num > y.num)
                        return -1;
                    else if (x.num < y.num)
                        return 1;
                    if (x.first < y.first)
                        return -1;
                    else if (x.first > y.first)
                        return 1;

                    return 0;
                });
            //list.Sort(new sortIntCompare());
            list.ForEach(i => Console.WriteLine(i.no + "--" + i.num + "--" + i.first + " "));
            //var t = list.OrderByDescending(l=>l.num).ThenBy(l => l.first);

            //t.ToList().ForEach(i => Console.WriteLine(i.no+"--"+i.num+"--"+i.first+" "));
        }

        private static int count = 0;
        //循环的次数
        private static int loopTime = 0;
        //保存结果的容器
        private static HashSet<String> ls = new HashSet<String>();
        public static void rotate(int[] result, List<int> al, int index)
        {
            index = result.Length - index;
            if (index == result.Length - 1)
            {
                ++loopTime;
                result[index] = al[0];
                organizeNum(result);
            }
            for (int s1 = 0; s1 < al.Count; s1++)
            {
                result[index] = al[s1];
                List<int> al1 = new List<int>(al);
                al1.Remove(result[index]);
                rotate(result, al1, al1.Count);
            }
        }
        //加入容器
        public static void organizeNum(int[] result)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString());
            }
            String pstr = sb.ToString();

            ls.Add(pstr);

        }

        // 组合函数
        static void com(int n, int k)
        {// 1-n里面取k个组合
            if (n < k || n <= 0 || k <= 0)
            {
                Console.WriteLine("n,k数据输入不合理");
                return;
            }
            int[] a = new int[k + 1];
            int[] fg = new int[k + 1];// 标记对照
            int count = 1;

            for (int i = 1; i <= k; i++)
            {
                a[i] = i;
                fg[i] = i - k + n;// 12345,3,fg就是(345)
            }
            while (true)
            {
                Console.WriteLine("第" + (count++) + ":\t");
                for (int i = 1; i <= k; i++)
                    Console.WriteLine(a[i] + "\t");
                Console.WriteLine();

                if (a[1] == n - k + 1)
                    break;// 跳出条件

                for (int i = k; i >= 1; i--)
                {                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      
                    if (a[i] < fg[i])
                    {// 那个数小于标记,加一
                        a[i]++;// 123456789,3589->3678(5->6,6+1,6+1+1)
                        for (int j = i + 1; j <= k; j++)
                            a[j] = a[j - 1] + 1;// 后面的数,依次加一升序
                        break;
                    }// if
                }// for i

            }// while

        }

        static void setCal()
        {

            int[] arr = { 1, 2, 3, 4, 5 };

            List<int[]> list = new List<int[]>();
            list.Add(arr);
            list.Add(arr);
            list.Add(arr);


        }

        static void cal(int[] res,int n)
        {
            if (ls == null)
                ls = new HashSet<String>();
            if (ls.Count < 1)
            {
                foreach (var item in res)
                {
                    ls.Add(item.ToString());
                }
            }
            else 
            {
                if (ls.ToList()[0].Length == n)
                    return;

                HashSet<String> tmp = new HashSet<string>();
                foreach (var l in ls)
                {
                    foreach (var r in res)
	                {
                        if (l.Contains(r.ToString()))
                            continue;
                        tmp.Add(l + r.ToString());
	                }
                }
                ls = tmp;
            }
            cal(res,n);
        }

    }

    public class sortInt
    {
        public int no;
        public int num;
        public int first;

        public sortInt(int no,int num,int f)
        {
            this.no = no;
            this.num = num;
            this.first = f;
        }
    }

    public class sortIntCompare : IComparer<sortInt>
    {

        public int Compare(sortInt x, sortInt y)
        {
            if (x.no == y.no && x.num == y.num)
                return 0;

            if (x.num > y.num)
                return -1;
            else if(x.num<y.num)
                return 1;
            if (x.first < y.first)
                return -1;
            else if (x.first > y.first)
                return 1;

            return 0;
        }
    }
}
