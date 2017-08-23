import React from 'react';
import { Link } from 'react-router';
import Photo from './Photo';
import Comments from './Comments';

const Single = React.createClass({
  render() {
    const { postId } = this.props.params;
    // index of post
    const i = this.props.posts.findIndex((post) => post.code === postId);
    // get us the post
    const post = this.props.posts[i];
    // get comments
    const postComments = this.props.comments[postId] || [];

    return (
      <div className="single-photo">
        <Photo i={i} post={post} {...this.props} />
        <Comments postComments={postComments} {...this.props} />
      </div>
    )
  }
})

export default Single;
