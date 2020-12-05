<template>
  <Suspense>
    <template #default>
      <CMarsLander/>
    </template>
    <template #fallback>
      Loading
    </template>
  </Suspense>
</template>

<script lang="ts">
import {defineComponent} from 'vue';
import CMarsLander from "@/views/mars-lander/components/CMarsLander.vue";

export default defineComponent({
  name: 'MarsLander',
  components: {CMarsLander},
})
</script>

<style scoped lang="scss">

</style>