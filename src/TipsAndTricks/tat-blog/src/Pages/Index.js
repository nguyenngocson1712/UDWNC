// import React, { useEffect,useState } from 'react';
import React, { useEffect } from "react";
// import PostItem from '.../Components/PostItem';
// import { getPosts } from '../Services/BlogRepository';

const Index = () => {
  // const [postList,setPostList]=useState([]);

  useEffect(() => {
    document.title = "Trang chủ";
    //  getPosts().then(data => {
    //     if (data)
    //     setPostList(data.items);
    //     else
    //     setPostList([]);
    //     })
  }, []);

  return <h1>Đây là trang chủ</h1>;
};

// if (postList.length > 0)
//  return (
//  <div className='p-4'>
//  {postList.map(item ,index=> {
//  return (
//  <PostItem postItem={item} />
//  );
//  })};
//  </div>
//  );
//  else return (
//  <></>
//  );
// }

export default Index;
