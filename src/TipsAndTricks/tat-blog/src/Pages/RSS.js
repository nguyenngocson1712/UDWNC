import React, { useEffect } from "react";

const RSS = () => {
  useEffect(() => {
    document.title = "Rss";
  }, []);

  return <h1>Đây là trang rss</h1>;
};

export default RSS;
