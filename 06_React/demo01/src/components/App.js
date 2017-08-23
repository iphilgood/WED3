import React, { Component } from 'react';
import '../App.css';

import Clock from './Clock';
import Toggle from './Toggle';
import LoginControl from './LoginControl';
import Calculator from './Calculator';
import FilterabelProductTable from './FilterableProductTable';

import { products } from '../data/products'

class App extends Component {
  render() {
    return (
      <div>
        <FilterabelProductTable products={products} />
        {/* <Calculator /> */}
        {/* <Clock /> */}
        {/* <Toggle /> */}
        {/* <LoginControl /> */}
      </div>
    );
  }
}

export default App;
