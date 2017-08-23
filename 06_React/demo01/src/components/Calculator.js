import React from 'react';

import TemperatureInput from './TemperatureInput';

function BoilingVerdict(props) {
  if (props.celsius >= 100) {
    return <p>The water would boil.</p>
  } else {
    return <p>The water would NOT boil.</p>
  }
}

function toCelsius(fahrenheit) {
  return (fahrenheit - 32) * 5 / 9;
}

function toFahrenheit(celsius) {
  return (celsius * 9 / 5) + 32;
}

function tryConvert(temperature, convert) {
  const input = parseFloat(temperature);
  if (Number.isNaN(input)) {
    return '';
  }
  const output = convert(input);
  const rounded = Math.round(output * 1000) / 1000;
  return rounded.toString();
}

class Calculator extends React.Component {
  constructor(props) {
    super(props)
    this.state = {scale: 'c', temp: ''};
  }

  handleCelsiusChange = (value) => {
    this.setState({scale: 'c', temp: value});
  }

  handleFahrenheitChange = (value) => {
    this.setState({scale: 'f', temp: value});
  }

  render() {
    const { scale, temp } = this.state;
    const celsius = scale === 'f' ? tryConvert(temp, toCelsius) : temp;
    const fahrenheit = scale === 'c' ? tryConvert(temp, toFahrenheit) : temp;

    return (
      <div>
        <TemperatureInput temp={celsius} scale="c" onChange={this.handleCelsiusChange} />
        <TemperatureInput temp={fahrenheit} scale="f" onChange={this.handleFahrenheitChange} />

        <BoilingVerdict celsius={parseFloat(celsius)} />
      </div>
    )
  }
}

export default Calculator;
