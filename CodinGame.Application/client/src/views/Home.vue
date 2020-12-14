<template>
  <label for="site">Select a site</label>
  <select id="site" v-model="site" @change="onSiteSelected">
    <option v-for="site in sites" :value="site.url">{{ site.name }}</option>
  </select>
</template>

<script lang="ts">
import {defineComponent, ref} from 'vue';
import {useRouter} from "vue-router";

export default defineComponent({
  name: 'Home',
  setup() {
    const site = ref('');
    const sites = [
      {name: 'Mars Lander', url: 'mars-lander'},
      {name: 'The Great Escape', url: 'great-escape'}
    ]
    const router = useRouter();

    function onSiteSelected() {
      router.push(site.value);
    }

    return {site, sites, onSiteSelected};
  }
})
</script>
