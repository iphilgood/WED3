import React from 'react';

class ProductCategoryRow extends React.Component {
  render() {
    return <tr><th colSpan="2">{this.props.category}</th></tr>;
  }
}

class ProductRow extends React.Component {
  render() {
    var name = this.props.product.stocked ?
      this.props.product.name :
      <span style={{color: 'red'}}>
        {this.props.product.name}
      </span>;
    return (
      <tr>
        <td>{name}</td>
        <td>{this.props.product.price}</td>
      </tr>
    );
  }
}

class ProductTable extends React.Component {
  render() {
    const rows = [];
    let lastCategory = null;

    this.props.products.forEach((product) => {
      if (product.name.indexOf(this.props.filterText) === -1 || (!product.stocked && this.props.inStockOnly)) {
        return;
      }
      if (product.category !== lastCategory) {
        rows.push(<ProductCategoryRow category={product.category} key={product.category} />);
      }
      rows.push(<ProductRow product={product} key={product.name} />);
      lastCategory = product.category;
    });

    return (
      <table>
        <thead>
          <tr>
            <th>Name</th>
            <th>Price</th>
          </tr>
        </thead>
        <tbody>{rows}</tbody>
      </table>
    );
  }
}

class SearchBar extends React.Component {

  onFilterTextInputChanged = (event) => {
    this.props.onFilterTextInputChanged(event.target.value);
  }

  onInStockOnlyChanged = (event) => {
    this.props.onInStockOnlyChanged(event.target.checked);
  }

  render() {
    const { filterText, inStockOnly } = this.props;

    return (
      <form>
        <input type="text" placeholder="Search..." value={filterText} onChange={this.onFilterTextInputChanged} />
        <p>
          <input type="checkbox" checked={inStockOnly} onChange={this.onInStockOnlyChanged} />
          {' '}
          Only show products in stock
        </p>
      </form>
    );
  }
}

class FilterableProductTable extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      filterText: '',
      inStockOnly: false
    };
  }

  handleFilterTextInput = (value) => {
    console.log('Filter input changed: ' + value);
    this.setState({filterText: value});
  }

  handleInStockOnlyInput = (value) => {
    console.log('In stock only input changed: ' + value);
    this.setState({inStockOnly: value});
  }

  render() {
    const { filterText, inStockOnly } = this.state;

    return (
      <div>
        <SearchBar filterText={filterText}
                   inStockOnly={inStockOnly}
                   onFilterTextInputChanged={this.handleFilterTextInput}
                   onInStockOnlyChanged={this.handleInStockOnlyInput}
        />

        <ProductTable products={this.props.products}
                      filterText={filterText}
                      inStockOnly={inStockOnly}
        />
      </div>
    );
  }
}

export default FilterableProductTable;
