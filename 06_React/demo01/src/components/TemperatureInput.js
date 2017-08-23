import React from 'react';

const scaleNames = {
  c: 'Celsius',
  f: 'Fahrenheit'
}

class TemperatureInput extends React.Component {
  handleChange = (event) => {
    this.props.onChange(event.target.value);
  }

  render() {
    const { temp, scale } = this.props;

    return (
      <fieldset>
        <legend>Enter temp in {scaleNames[scale]}:</legend>
        <input type="text"
               value={temp}
               onChange={this.handleChange} />
      </fieldset>
    )
  }
}

export default TemperatureInput;
