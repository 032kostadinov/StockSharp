namespace SampleHistoryTestingParallel
{
	using Ecng.Common;

	using StockSharp.Algo;
	using StockSharp.Algo.Candles;
	using StockSharp.Algo.Indicators;
	using StockSharp.Algo.Strategies;
	using StockSharp.Messages;

	class SmaStrategy : Strategy
	{
		private readonly CandleSeries _series;
		private bool _isShortLessThenLong;

		private readonly StrategyParam<int> _longSmaPeriod;

		public int LongSmaPeriod
		{
			get { return _longSmaPeriod.Value; }
		}

		private readonly StrategyParam<int> _shortSmaPeriod;

		public int ShortSmaPeriod
		{
			get { return _shortSmaPeriod.Value; }
		}

		public SmaStrategy(CandleSeries series, SimpleMovingAverage longSma, SimpleMovingAverage shortSma)
		{
			_series = series;

			LongSma = longSma;
			ShortSma = shortSma;

			_longSmaPeriod = this.Param("LongSmaPeriod", longSma.Length);
			_shortSmaPeriod = this.Param("ShortSmaPeriod", shortSma.Length);
		}

		public SimpleMovingAverage LongSma { get; private set; }
		public SimpleMovingAverage ShortSma { get; private set; }

		protected override void OnStarted()
		{
			_series
				.WhenCandlesFinished()
				.Do(ProcessCandle)
				.Apply(this);

			this
				.GetCandleManager()
				.Start(_series);

			// ���������� ������� ��������� ������������ ���� �����
			_isShortLessThenLong = ShortSma.GetCurrentValue() < LongSma.GetCurrentValue();

			base.OnStarted();
		}

		private void ProcessCandle(Candle candle)
		{
			// ���� ���� ��������� � �������� ���������
			if (ProcessState == ProcessStates.Stopping)
			{
				// �������� �������� ������
				CancelActiveOrders();
				return;
			}

			// ��������� ����� �����
			LongSma.Process(candle);
			ShortSma.Process(candle);

			// ��������� ����� ��������� ������������ ���� �����
			var isShortLessThenLong = ShortSma.GetCurrentValue() < LongSma.GetCurrentValue();

			// ���� ��������� �����������
			if (_isShortLessThenLong != isShortLessThenLong)
			{
				// ���� �������� ������ ��� �������, �� �������, �����, �������.
				var direction = isShortLessThenLong ? Sides.Sell : Sides.Buy;

				// ��������� ������ ��� �������� ��� ���������� ����
				var volume = Position == 0 ? Volume : Position.Abs() * 2;

				// ������������ ������ (������� �������� - �������������� �������)
				RegisterOrder(this.CreateOrder(direction, (decimal)(Security.GetCurrentPrice(this, direction) ?? 0), volume));

				// �������������� ������� ����� �����������
				//var strategy = new MarketQuotingStrategy(direction, volume);
				//ChildStrategies.Add(strategy);

				// ���������� ������� ��������� ������������ ���� �����
				_isShortLessThenLong = isShortLessThenLong;
			}
		}
	}
}