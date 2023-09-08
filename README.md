# What is this?
A small program to generate static assets (html, CSS, js) for an FAA charts quick reference page. Inspired by https://charts.bvartcc.com/, using data and charts from https://www.aviationapi.com/.

![image](https://github.com/vzoa/charts-site-generator/assets/34892440/f2d6875a-c1bd-4a7d-aa6a-3738c6316313)

# How can I deploy my own?
1. Fork the repo to your own account/organization
2. Edit the airports in `site_config.yaml`, split into `Bravo`, `Charlie` and `Delta` categories
3. Set up a Cloudflare Pages app that is connected to your forked repo. Use `chmod +x ./build-cloudflare.sh; ./build-cloudflare.sh` as the build command and `/output/wwwroot` as the output directory
4. If you'd like, follow https://www.codemzy.com/blog/scheduling-builds-cloudflare to set up a daily cron so that your site we re-build with the latest charts daily, except replace the Cloudflare Worker code with the following for Cloudflare's newer ES format (and don't forget to insert your own Cloudflare Pages deploy hook)
```js
export default { 
  async scheduled(event, env, ctx) {
    let deployHook = "YOUR DEPLOY HOOK URL HERE";
    await fetch(deployHook, {
      method: "POST",
      headers: {
      "Content-Type": "application/json",
      },
    });
    return "Called deploy hook!"
  },
};
```
