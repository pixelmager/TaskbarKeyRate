# TaskbarKeyRate
Wrapper for SPI_GETFILTERKEYS to set custom key repeat/delay-rates, read: Setting higher repeat-rates and shorter delays than what is possible using the windows-native settings.

Testing shows RepeatRate can not go below 15ms between repeats - also, appears RepeatRate differs between alphanumeric keys and control-keys like pgdn/pgup/arrows.
![testimage](https://raw.githubusercontent.com/pixelmager/TaskbarKeyRate/master/KeyRateTest/output_test.jpg)
