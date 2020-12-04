<template>
  <div ref="map"/>
</template>

<script lang="ts">
import {defineComponent, ref, Ref, onMounted} from 'vue';
import {MarsLanderApi} from "@/api/MarsLanderApi";

export default defineComponent({
  name: 'MarsLander',
  async setup() {
    const map = ref(null) as Ref<null | HTMLDivElement>;

    const marsLanderApi = new MarsLanderApi();

    const mapElements = await marsLanderApi.GetMaps();

    console.dir(mapElements);
  }
})
</script>

<style scoped lang="scss">

</style>