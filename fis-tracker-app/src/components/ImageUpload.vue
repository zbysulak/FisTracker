<template>
  <v-card>
    <v-card-title>
      Screenshot Upload <v-spacer />
      <v-switch v-model="overwrite" label="Overwrite existing" />
    </v-card-title>
    <div>
      <div class="">
        <!--UPLOAD-->
        <form enctype="multipart/form-data" novalidate>
          <div class="dropbox">
            <input
              type="file"
              :name="uploadFieldName"
              @change="filesChange($event.target.files)"
              accept="image/*"
              class="input-file" />
            <p v-if="isUploading">
              Uploading your screenshot...
              <v-progress-linear
                indeterminate
                color="yellow darken-2"></v-progress-linear>
            </p>
            <p v-else>
              Drag your FIS screenshot here to begin<br />
              or click to browse<br />
              or just paste a screenshot
            </p>
          </div>
        </form>
      </div>
    </div>
  </v-card>
</template>

<!-- Javascript -->
<script>
import axios from "axios"

export default {
  data() {
    return {
      uploadedFiles: [],
      uploadError: null,
      currentStatus: null,
      uploadFieldName: "image",
      isUploading: false,
      overwrite: false
    }
  },
  methods: {
    handlePasteImage(e) {
      var items = e.clipboardData.items
      var blob = items[0].getAsFile()
      let frm = new FormData()
      frm.append("image", blob)
      this.upload(frm)
    },
    upload(formData) {
      formData.append("overwrite", this.overwrite)
      this.isUploading = true
      const url = this.appConfig.apiUrl + "/TimeInputs/parseimage"
      return axios
        .post(url, formData)
        .then((e) => {
          this.$root.snack.show({
            message: "Image successfully uploaded and parsed"
          })
          this.$emit("uploaded", e.data.maxDate)
        })
        .catch((e) => {
          console.error(e)
          this.$root.snack.show({
            message: "An error occured while processing image",
            color: "error",
            icon: "mdi-alert"
          })
        })
        .finally(() => {
          this.isUploading = false
        })
    },
    filesChange(fileList) {
      // handle file changes
      const formData = new FormData()

      if (fileList.length != 1) {
        this.$root.snack.show({
          message: "Invalid number of images",
          color: "error",
          icon: "mdi-alert"
        })
        return
      }
      formData.append("image", fileList[0], fileList[0].name)

      // save it
      this.upload(formData)
    }
  },
  mounted() {
    document.addEventListener("paste", this.handlePasteImage)
  }
}
</script>

<!-- SASS styling -->
<style lang="scss">
.dropbox {
  outline: 2px dashed grey; /* the dash box */
  outline-offset: -10px;
  background: lightcyan;
  color: dimgray;
  padding: 10px 10px;
  min-height: 200px; /* minimum height */
  position: relative;
  cursor: pointer;
}

.input-file {
  opacity: 0; /* invisible but it's there! */
  width: 100%;
  height: 200px;
  position: absolute;
  cursor: pointer;
}

.dropbox:hover {
  background: lightblue; /* when mouse over to the drop zone, change color */
}

.dropbox p {
  font-size: 1.2em;
  text-align: center;
  padding: 50px 0;
}
</style>
