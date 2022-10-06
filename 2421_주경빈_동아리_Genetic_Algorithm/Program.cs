using System;
using System.Collections.Generic;

namespace _2421_주경빈_동아리_Genetic_Algorithm
{
    class Password_Genetic
    {
        string Password;
        Random random;
        public List<string> generation;//세대
        Dictionary<string, float> generation_Score;//세대 점수
        int len;
        public int pass;
        public Password_Genetic()
        {
            pass = 0;
            Password = null;
            random = new Random();
            generation = new List<string>();
            generation_Score = new Dictionary<string, float>();
            SetPassword();
            Console.WriteLine("비밀번호 생성 : "+Password);

        }
        //적합성 검사 함수
        public float fitness(string _password,string _testword)
        {
            float score = 0;
            if (_password.Length != _testword.Length)
                return score;

            score += 0.5f;
            len = _testword.Length;
            for (int  i = 0;i< _password.Length-1;i++)
            {
                if (i > _testword.Length)
                    break;
                if (_password[i] == _testword[i])
                    score += 1;
            }//end for
            return score / (_password.Length+ 0.5f) * 100;
        }
        public bool Checkgeneration()
        {
            for(int i =0; i<generation.Count;i++)
            {
                if (generation [i]== Password)
                {
                    pass = i;
                    return true;
                }
            }//end for
            return false;
        }
        //세대를 적합성 검사(fitness()) 해보고 점수에 따라 정렬하는 함수
        public void compute_performance()
        {
            generation_Score = new Dictionary<string, float>();
            List<int> performance_list = new List<int>();
            float score = 0;
            for(int i=0;i< generation.Count;i++)
            {
                score = fitness(Password, generation[i]);

                //if(score>0)
                //{
                //    //예상 길이 정하기
                    
                //}
                if(!generation_Score.ContainsKey(generation[i]))
                    generation_Score.Add(generation[i], score);
            }//end for
            //정렬 (퀵 정렬)
            quickSort(0, generation.Count - 1);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("--------------------------[");
            Console.Write("'" + generation[0] + ": " + generation_Score[generation[0]] + "' ");
            Console.Write("]-");
        }
        //살아남을 세대를 선택하는 함수
        public List<string> Select_survivors(int best_sample,int lucky_few)
        {
            List<string> next_generation = new List<string>();
            for(int i=0; i<best_sample;i++)
            {
                if(generation_Score[ generation[i]]>0)
                {
                    next_generation.Add(generation[i]);
                }
            }//end for
            for (int i = 0; i < lucky_few; i++)
            {
                next_generation.Add(generation[random.Next(best_sample,generation.Count)]);
            }//end for
            if(next_generation.Count<best_sample+lucky_few)
            {
                for(int i=0;i<(best_sample+lucky_few)-next_generation.Count;i++)
                {
                    string s = null;
                    for(int j=0;j<random.Next(2,11);j++)
                    {
                        s += GetRandom();
                    }
                    next_generation.Add(s);
                }//end for
            }

            return next_generation;
            #region 함수의 결과물 확인
            //Console.WriteLine("");
            //Console.WriteLine("Select------------------------------------");
            //Console.Write("[");
            //foreach (var c in next_generation)
            //{
            //    Console.Write("'" + c + "', ");
            //}
            //Console.Write("]");
            #endregion
        }

        //새로운 세대를 만드는 함수
        public void SetGeneration(int size,int min_len,int max_len)
        {
            int L;
            string word;
            for (int i=0;i<size;i++)
            {
                L = random.Next(min_len, max_len+1);
                word = null;
                for (int j=0;j<L;j++)
                {
                    word+= GetRandom();
                }//end for
                generation.Add(word);
            }//end for
        }
        public void Create_children(int n_child)
        {
            List<string> next_generation = new List<string>();
            for(int i=0;i< generation.Count/2;i++)
            {
                for(int j=0;j<n_child;j++)
                {
                    next_generation.Add(create_child(generation[i], generation[generation.Count - 1 - i]));
                }//end for
            }//end for
            generation = next_generation;
        }
        private string create_child(string gene1,string gene2)
        {
            string child = null;
            for(int i=0;i<len;i++)
            {
                if(random.Next(0,101)<50)
                {
                    if (i > gene1.Length-1)
                    {
                        if (i > gene2.Length-1)
                            return child;
                        child += gene2[i];
                        continue;
                    }
                    child += gene1[i];
                }
                else
                {
                    if(i>gene2.Length-1)
                    {
                        if (i > gene1.Length-1)
                            return child;
                        child += gene1[i];
                        continue;
                    }
                    child += gene2[i];
                }
            }//end for
            return child;
        }

        #region 돌연변이-mutate
        public string mutate_word(string word)
        {
            int idx = random.Next(0, Password.Length);
            int i = random.Next(0, word.Length);
            word = word.Insert(i, Password[idx].ToString());

            return word;
        }
        public void muate_generation()
        {
            for(int i=0;i< generation.Count;i++)
            {
                if(random.Next(0,100)<=10)
                {
                   
                    generation[i] = mutate_word(generation[i]);
                }
            }
        }
        #endregion
        //현재 세대를 출력하는 함수
        public void PrintGeneration()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("[");
            foreach(var c in generation)
            {
                Console.Write("'" + c + "', ");
            }//end foreach
            Console.Write("]");
        }
        //비밀번호를 생성하는 함수
        private void SetPassword()
        {
            int L = random.Next(2, 11);
            for (int i = 0; i < L; i++)
            {
                Password += GetRandom();
            }//end for

        }
        //랜덤한 문장을 넣어주는 함수 / 0 ~ 9, a ~ z, A ~ Z
        private char GetRandom()
        {
            int ch = random.Next(1, 4);
            char s = ' ';
            switch (ch)
            {
                case 1://숫자
                    s = (char)random.Next(48, 58);
                    break;
                case 2://영어 대문자
                    s = (char)random.Next(65, 91);
                    break;
                case 3://영어 소문자 
                    s = (char)random.Next(97, 123);
                    break;
            }//end switch
            return s;
        }
        //세대를 점수에 따라 정렬하는 퀵정렬 함수
        private void quickSort(int start,int end)
        {
            //정렬의 끝
            if (start >= end)
                return;
            int pivot = start;
            int left = pivot + 1;
            int right = end;

            string temp;
            int count = 0;
            while(left<=right)
            {
                while (left <= end&& generation_Score[generation[left]] >= generation_Score[generation[pivot]])
                {
                    //피벗과 같은 값 갯수 확인
                    if (generation_Score[generation[left]] == generation_Score[generation[pivot]])
                    {
                        count++;
                    }
                    left++;
                }//end while
                while (right > start && generation_Score[generation[right]] <= generation_Score[generation[pivot]])
                {
                    right--;
                }//end while
                //모든 요소가 같으므로 종료 return
                if (count == end)
                {
                    return;
                }


                if (left>right)
                {
                    temp = generation[right];
                    generation[right] = generation[pivot];
                    generation[pivot] = temp;
                }
                else
                {
                    temp = generation[right];
                    generation[right] = generation[left];
                    generation[left] = temp;
                }

            }//end while
            quickSort(start, right - 1);
            quickSort(right + 1, end);
        }// end quickSort
    }//end class
    class Program
    {
        static void Main(string[] args)
        {
            //클래스 생성 & 비밀번호 생성
            Password_Genetic _Genetic = new Password_Genetic();
            int i = 1;

            //세대 생성
            _Genetic.SetGeneration(100, 2, 10);

            //세대 적합도 검사
            _Genetic.compute_performance();

            //세대 선택
            _Genetic.generation = _Genetic.Select_survivors(20, 20);
            Console.WriteLine(i);

            //반복
            while(true)
            {
                
                _Genetic.Create_children(10);

                _Genetic.muate_generation();
                _Genetic.compute_performance();
                Console.WriteLine(i);
                if (_Genetic.Checkgeneration())
                {
                    break;
                }
                _Genetic.generation = _Genetic.Select_survivors(20, 20);
                i++;
            }//end for

            //종료
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine((i+1)+"번째 : "+_Genetic.generation[_Genetic.pass]);
        }
    }
}
