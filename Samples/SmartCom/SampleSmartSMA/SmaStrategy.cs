namespace SampleSmartSMA
{
	using Ecng.Common;

	using StockSharp.Algo;
	using StockSharp.Algo.Candles;
	using StockSharp.Algo.Indicators;
	using StockSharp.Algo.Strategies;
	using StockSharp.Algo.Strategies.Quoting;
	using StockSharp.Messages;

	class SmaStrategy : Strategy
	{
		private readonly ICandleManager _candleManager;
		private readonly CandleSeries _series;
		private bool _isShortLessThenLong;

		public SmaStrategy(ICandleManager candleManager, CandleSeries series, SimpleMovingAverage longSma, SimpleMovingAverage shortSma)
		{
			_candleManager = candleManager;
			_series = series;

			LongSma = longSma;
			ShortSma = shortSma;
		}

		public SimpleMovingAverage LongSma { get; }
		public SimpleMovingAverage ShortSma { get; }

		protected override void OnStarted()
		{
			_candleManager
				.WhenCandlesFinished(_series)
				.Do(ProcessCandle)
				.Apply(this);

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
				//RegisterOrder(this.CreateOrder(direction, (decimal)Security.GetCurrentPrice(direction), volume));

				// �������������� ������� ����� �����������
				var strategy = new MarketQuotingStrategy(direction, volume);
				ChildStrategies.Add(strategy);

				// ���������� ������� ��������� ������������ ���� �����
				_isShortLessThenLong = isShortLessThenLong;
			}
		}
	}
}