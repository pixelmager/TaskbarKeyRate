#include <stdio.h>
#include <stdint.h>
#include <conio.h>

/////////////////////////////////////////////////////////

#define WIN32_LEAN_AND_MEAN
#define NOMINMAX
#include <Windows.h>

LARGE_INTEGER currentTime;
LARGE_INTEGER m_timer_frequency;
void init_timers()
{
	QueryPerformanceFrequency( &m_timer_frequency );
}
uint64_t gettime_ms()
{
	QueryPerformanceCounter(&currentTime);
	return static_cast<uint64_t>( static_cast<double>(currentTime.QuadPart) / static_cast<double>(m_timer_frequency.QuadPart) * 1000.0 );
}

/////////////////////////////////////////////////////////
//note: see also http://stereopsis.com/keyrepeat/
//
//note: rate different for arrow-keys / letters?

int main()
{
	printf( "counts keys/s (esc / ctrl+c to exit)\n");

	init_timers();

	uint64_t starttime = gettime_ms();
	uint64_t prevtime = starttime;
	uint64_t firsttime = 0;
	int cnt = 0;
	for(;;)
	{
		_getch();
		
		++cnt;

		uint64_t curtime = gettime_ms();
		

		if ( curtime - prevtime > 1000 )
		{
			firsttime = curtime;
		}
		prevtime = curtime;

		if ( firsttime != curtime && firsttime != 0 )
		{
			uint64_t delay = curtime - firsttime;
			printf( "delay=%lldms\n", delay );
			firsttime = 0;
			starttime = curtime;
			cnt = 0;
		}

		uint64_t dt_ms = curtime - starttime;

		if ( dt_ms > 2000 && dt_ms > 0 && cnt > 0 )
		{
			float c_per_ms = (float)cnt / (float)dt_ms;
			float ms_per_c = (float)dt_ms / (float)cnt;
			printf("%.2f chars/s (rate=%.2fms)\n", 1000.0f*c_per_ms, ms_per_c );
			starttime = curtime;
			cnt = 0;
		}
		
	}

	return 0;
}
