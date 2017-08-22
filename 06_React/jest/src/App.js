import React, { Component } from 'react';
import logo from './logo.svg';
import './App.css';

import CheckboxWithLabel from './CheckboxWithLabel';
import Link from './Link';

class App extends Component {
  render() {
    return (
      <div className="App">
        <div className="App-header">
          <img src={logo} className="App-logo" alt="logo" />
          <h2>Welcome to React</h2>
        </div>
        <p className="App-intro">
          To get started, edit <code>src/App.js</code> and save to reload.
        </p>

        <h2>Ultimate Checkbox with label</h2>
        <CheckboxWithLabel labelOn="On" labelOff="Off" />

        <h2>Awesome Link Component, bruuuuuuh</h2>
        <Link page="http://www.facebook.com">Facebook</Link>
      </div>
    );
  }
}

export default App;
