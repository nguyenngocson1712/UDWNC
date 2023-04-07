import React, { useEffect } from "react";

const About = () => {
  useEffect(() => {
    document.title = "Trang About";
  }, []);

  return <h1>Đây là trang About</h1>;
};

export default About;
