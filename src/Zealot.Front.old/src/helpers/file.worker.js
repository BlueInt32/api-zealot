console.log('Hello ! I am a worker :)');
onmessage = (event) => {
  const { code, context } = event.data;
  console.log(`Worker: received code '${code}' and context :`);
  try {
    eval(code); // eslint-disable-line
  } catch (error) {
    console.log(error);
    postMessage({ error });
  }

  console.log(context);
  postMessage({ context });
};
