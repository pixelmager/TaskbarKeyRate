#define WIN32_LEAN_AND_MEAN
#define NOMINMAX
#include <Windows.h>

extern "C" __declspec(dllexport) void SetKeyRate( int repeatrate_ms, int delay_ms )
{
	FILTERKEYS keys = { sizeof(FILTERKEYS) };
	
	if ( repeatrate_ms > 0 && delay_ms > 0 )
	{
		keys.dwFlags = FKF_FILTERKEYSON|FKF_AVAILABLE;
		keys.iRepeatMSec = repeatrate_ms; // Repeat Rate
		keys.iDelayMSec = delay_ms; // Delay Until Repeat

		keys.iWaitMSec = 0; // Acceptance Delay
		keys.iBounceMSec = 0; // Debounce Time
	}

	SystemParametersInfo (SPI_SETFILTERKEYS, 0, (LPVOID) &keys, 0);
}

//extern "C" __declspec(dllexport) int GetRepeatRateMs()
//{
//	FILTERKEYS getkeys;
//	getkeys.cbSize = sizeof(FILTERKEYS);
//	if ( SystemParametersInfo (SPI_GETFILTERKEYS, sizeof(FILTERKEYS), (LPVOID)&getkeys, 0) )
//	{
//		return getkeys.iRepeatMSec;
//	}
//	return -1;
//}
//
//extern "C" __declspec(dllexport) int GetDelayMs()
//{
//	FILTERKEYS getkeys;
//	getkeys.cbSize = sizeof(FILTERKEYS);
//	if ( SystemParametersInfo (SPI_GETFILTERKEYS, sizeof(FILTERKEYS), (LPVOID)&getkeys, 0) )
//	{
//		return getkeys.iDelayMSec;
//	}
//	return -1;
//}
