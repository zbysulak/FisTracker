<template>
  <v-row class="text-center copy mb-0" justify="center">Created with  <v-icon>{{ heartIcon }} </v-icon> by Zbyšek & Martin</v-row>
</template>

<script>
export default {
  name: "footer",
  data: () => ({
    heartIcon: "mdi-cards-heart-outline"
  })
}
</script>

<style scoped>
    i {
        color: red !important;
        font-size: 17px !important;
        margin-left: 2px;
        margin-right: 2px;
    }
    .copy {
        font-size: 14px;
        color: #708094;
    }
</style>