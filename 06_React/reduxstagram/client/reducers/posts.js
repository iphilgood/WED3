// a redurcer takes in two things:

// 1. The action (info what happened)
// 2. Copy of current state

function posts(state = [], action) {
  switch(action.type) {
    case 'INCREMENT_LIKES':
      console.log('Incrementing likes');
      const i = action.index;
      return [
        ...state.slice(0, i), // beofre the one we are updating
        {...state[i], likes: state[i].likes + 1},
        ...state.slice(i + 1), // after the one we are updating
      ]
    default:
      return state;
  }
  return state;
}

export default posts;
