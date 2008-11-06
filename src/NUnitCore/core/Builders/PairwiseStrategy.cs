// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.Collections;
using System.Reflection;
using NUnit.Core.Extensibility;

namespace NUnit.Core.Builders
{
	public class PairwiseStrategy : CombiningStrategy
	{
		internal class Pair
		{
			private int dimension1;
			private int dimension2;
			private int value1;
			private int value2;
			private bool covered;

			public Pair(int dimension1, int dimension2, int value1, int value2)
			{
				this.dimension1 = dimension1;
				this.dimension2 = dimension2;
				this.value1 = value1;
				this.value2 = value2;
			}

			public int Dimension1
			{
				get
				{
					return this.dimension1;
				}
			}

			public int Dimension2
			{
				get
				{
					return this.dimension2;
				}
			}

			public int Value1
			{
				get
				{
					return this.value1;
				}
			}

			public int Value2
			{
				get
				{
					return this.value2;
				}
			}

			public bool Covered
			{
				get
				{
					return this.covered;
				}

				set
				{
					this.covered = value;
				}
			}
		}

		internal class PairwiseTestCase
		{
			private int[] testData;

			public PairwiseTestCase(int[] testData)
			{
				this.testData = testData;
			}

			public int[] TestData
			{
				get
				{
					return this.testData;
				}
			}
		}

		internal class PairwiseTestCaseGenerator
		{
			private int[] dimensions;
			private Pair[] pairs;
			private ArrayList testCases = new ArrayList();
			private Random random = new Random(0);

			public ArrayList Generate(int[] dimensions)
			{
				this.dimensions = dimensions;

				this.CreatePairs();

				this.CreateTestCases();

				this.SelfTest();

				return this.testCases;
			}

			private void CreatePairs()
			{
				this.pairs = new Pair[CountPairs()];

				int index = 0;

				for (int dimension1 = 0; dimension1 < this.dimensions.Length - 1; dimension1++)
				{
					for (int dimension2 = dimension1 + 1; dimension2 < this.dimensions.Length; dimension2++)
					{
						for (int value1 = 0; value1 < this.dimensions[dimension1]; value1++)
						{
							for (int value2 = 0; value2 < this.dimensions[dimension2]; value2++)
							{
								this.pairs[index++] = new Pair(dimension1, dimension2, value1, value2);
							}
						}
					}
				}
			}

			private int CountPairs()
			{
				int pairCount = 0;

				for (int dimension1 = 0; dimension1 < this.dimensions.Length - 1; dimension1++)
				{
					for (int dimension2 = dimension1 + 1; dimension2 < this.dimensions.Length; dimension2++)
					{
						pairCount += this.dimensions[dimension1] * this.dimensions[dimension2];
					}
				}

				return pairCount;
			}

			private void CreateTestCases()
			{
				while (true)
				{
					PairwiseTestCase testCase = this.CreateTestCase();

					if (testCase == null)
					{
						break;
					}

					this.UpdateCoverage(testCase);

					this.testCases.Add(testCase);
				}
			}

			private PairwiseTestCase CreateTestCase()
			{
				int pairsCovered = 0;

				int[] testData = new int[this.dimensions.Length];

				bool[] dimensionCovered = new bool[this.dimensions.Length];

				while (true)
				{
					ArrayList uncoveredPairs = new ArrayList();

					for (int i = 0; i < this.pairs.Length; i++)
					{
						Pair pair = this.pairs[i];

						if (pair.Covered)
						{
							continue;
						}

						if (dimensionCovered[pair.Dimension1] && testData[pair.Dimension1] != pair.Value1)
						{
							continue;
						}

						if (dimensionCovered[pair.Dimension2])
						{
							continue;
						}

						uncoveredPairs.Add(pair);
					}

					if (uncoveredPairs.Count == 0)
					{
						break;
					}

					Pair uncoveredPair = (Pair)uncoveredPairs[this.random.Next(uncoveredPairs.Count)];

					testData[uncoveredPair.Dimension1] = uncoveredPair.Value1;
					testData[uncoveredPair.Dimension2] = uncoveredPair.Value2;

					dimensionCovered[uncoveredPair.Dimension1] = true;
					dimensionCovered[uncoveredPair.Dimension2] = true;

					uncoveredPair.Covered = true;

					pairsCovered++;
				}

				if (pairsCovered == 0)
				{
					return null;
				}

				return new PairwiseTestCase(testData);
			}

			private void UpdateCoverage(PairwiseTestCase testCase)
			{
				for (int dimension1 = 0; dimension1 < this.dimensions.Length - 1; dimension1++)
				{
					for (int dimension2 = dimension1 + 1; dimension2 < this.dimensions.Length; dimension2++)
					{
						for (int i = 0; i < this.pairs.Length; i++)
						{
							Pair pair = this.pairs[i];

							if (pair.Dimension1 == dimension1
								&& pair.Value1 == testCase.TestData[dimension1]
								&& pair.Dimension2 == dimension2
								&& pair.Value2 == testCase.TestData[dimension2])
							{
								pair.Covered = true;
								break;
							}
						}
					}
				}
			}

			private void SelfTest()
			{
				bool[] pairsCovered = new bool[this.pairs.Length];

				foreach (PairwiseTestCase testCase in this.testCases)
				{
					for (int dimension1 = 0; dimension1 < this.dimensions.Length - 1; dimension1++)
					{
						for (int dimension2 = dimension1 + 1; dimension2 < this.dimensions.Length; dimension2++)
						{
							for (int i = 0; i < this.pairs.Length; i++)
							{
								Pair pair = this.pairs[i];

								if (pair.Dimension1 == dimension1
									&& pair.Value1 == testCase.TestData[dimension1]
									&& pair.Dimension2 == dimension2
									&& pair.Value2 == testCase.TestData[dimension2])
								{
									pairsCovered[i] = true;
									break;
								}
							}
						}
					}
				}

				for (int i = 0; i < pairsCovered.Length; i++)
				{
					if (!pairsCovered[i])
					{
						throw new ApplicationException("PairwiseStrategy self-test failed : Not all pairs are covered!");
					}
				}
			}
		}

		public PairwiseStrategy(IEnumerable[] sources) : base(sources) { }

		public override IEnumerable GetTestCases()
		{
			ArrayList[] valueSet = CreateValueSet();
			int[] dimensions = CreateDimensions(valueSet);

			ArrayList pairwiseTestCases = new PairwiseTestCaseGenerator().Generate(dimensions);

#if !NET_2_0
			ArrayList testCases = new ArrayList();
#endif

			foreach (PairwiseTestCase pairwiseTestCase in pairwiseTestCases)
			{
				object[] testData = new object[pairwiseTestCase.TestData.Length];

				for (int i = 0; i < pairwiseTestCase.TestData.Length; i++)
				{
					testData[i] = valueSet[i][pairwiseTestCase.TestData[i]];
				}

#if NET_2_0
				yield return testData;
#else
				testCases.Add(testData);
#endif
			}

#if !NET_2_0
			return testCases;
#endif
		}

		private ArrayList[] CreateValueSet()
		{
			ArrayList[] valueSet = new ArrayList[Sources.Length];

			for (int i = 0; i < valueSet.Length; i++)
			{
				ArrayList values = new ArrayList();

				foreach (object value in Sources[i])
				{
					values.Add(value);
				}

				valueSet[i] = values;
			}

			return valueSet;
		}

		private int[] CreateDimensions(ArrayList[] valueSet)
		{
			int[] dimensions = new int[valueSet.Length];

			for (int i = 0; i < valueSet.Length; i++)
			{
				dimensions[i] = valueSet[i].Count;
			}

			return dimensions;
		}
	}
}
